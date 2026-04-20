using UnityEngine;

[RequireComponent(typeof(CombatStats))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    [Header("Combat Settings")]
    [SerializeField] private float attackRange = 2.2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float followStopOffset = 1.5f;

    private CombatStats combatStats;
    private PlayerMotor playerMotor;
    private Targetable currentTarget;
    private float lastAttackTime;

    private void Awake()
    {
        combatStats = GetComponent<CombatStats>();
        playerMotor = GetComponent<PlayerMotor>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        HandleTargetSelection();
        HandleAutoAttack();
    }

    private void HandleTargetSelection()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (mainCamera == null)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 200f))
            return;

        Targetable target = hit.collider.GetComponent<Targetable>();

        if (target == null)
        {
            target = hit.collider.GetComponentInParent<Targetable>();
        }

        if (target == null)
            return;

        if (!target.CanBeTargeted())
            return;

        currentTarget = target;

        Debug.Log($"Alvo selecionado: {currentTarget.gameObject.name}");
    }

    private void HandleAutoAttack()
    {
        if (currentTarget == null)
            return;

        if (!currentTarget.CanBeTargeted())
        {
            Debug.Log("Alvo inválido ou morto.");
            currentTarget = null;
            playerMotor.StopMovement();
            return;
        }

        Vector3 targetPosition = currentTarget.transform.position;
        Vector3 flatTargetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        float distance = Vector3.Distance(transform.position, flatTargetPosition);

        if (distance > attackRange)
        {
            Vector3 direction = (flatTargetPosition - transform.position).normalized;
            Vector3 desiredStopPosition = flatTargetPosition - direction * followStopOffset;

            playerMotor.MoveTo(desiredStopPosition);
            return;
        }

        playerMotor.StopMovement();

        Vector3 lookDirection = flatTargetPosition - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 12f * Time.deltaTime);
        }

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        CombatStats targetStats = currentTarget.CombatStats;

        if (targetStats == null)
        {
            Debug.LogWarning("O alvo não possui CombatStats.");
            currentTarget = null;
            return;
        }

        lastAttackTime = Time.time;

        Debug.Log($"{gameObject.name} atacou {currentTarget.gameObject.name} causando {combatStats.AttackDamage} de dano.");
        targetStats.TakeDamage(combatStats.AttackDamage);
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }
}