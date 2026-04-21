using UnityEngine;

[RequireComponent(typeof(CombatStats))]
[RequireComponent(typeof(CharacterController))]
public class SkeletonCombat : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 8f;

    [Header("Combat")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.2f;

    private CombatStats combatStats;
    private CharacterController controller;
    private SkeletonAnimation skeletonAnimation;

    private float lastAttackTime;
    private float verticalVelocity;
    private const float Gravity = -9.81f;

    public bool HasTarget => target != null;

    private void Awake()
    {
        combatStats = GetComponent<CombatStats>();
        controller = GetComponent<CharacterController>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void Update()
    {
        HandleGravity();

        if (target == null || combatStats.IsDead)
        {
            skeletonAnimation?.SetMoving(false);
            ApplyVerticalOnly();
            return;
        }

        Vector3 targetPosition = target.position;
        Vector3 flatTargetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        float distance = Vector3.Distance(transform.position, flatTargetPosition);

        if (distance > attackRange)
        {
            MoveToTarget(flatTargetPosition);
        }
        else
        {
            skeletonAnimation?.SetMoving(false);
            FaceTarget(flatTargetPosition);
            TryAttack();
            ApplyVerticalOnly();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (combatStats.IsDead) return;

        target = newTarget;
        Debug.Log($"{gameObject.name} agora está mirando em {target.name}");
    }

    public void ClearTarget()
    {
        target = null;
        skeletonAnimation?.SetMoving(false);
    }

    private void MoveToTarget(Vector3 flatTargetPosition)
    {
        Vector3 direction = flatTargetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            skeletonAnimation?.SetMoving(false);
            ApplyVerticalOnly();
            return;
        }

        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        Vector3 move = direction * moveSpeed;
        move.y = verticalVelocity;

        skeletonAnimation?.SetMoving(true);
        controller.Move(move * Time.deltaTime);
    }

    private void FaceTarget(Vector3 flatTargetPosition)
    {
        Vector3 direction = flatTargetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        CombatStats targetStats = target.GetComponent<CombatStats>();

        if (targetStats == null)
            targetStats = target.GetComponentInParent<CombatStats>();

        if (targetStats == null || targetStats.IsDead)
        {
            ClearTarget();
            return;
        }

        lastAttackTime = Time.time;

        skeletonAnimation?.PlayAttack();

        Debug.Log($"{gameObject.name} atacou {target.name} causando {combatStats.AttackDamage} de dano.");
        targetStats.TakeDamage(combatStats.AttackDamage);
    }

    private void HandleGravity()
    {
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void ApplyVerticalOnly()
    {
        Vector3 move = Vector3.zero;
        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }
}