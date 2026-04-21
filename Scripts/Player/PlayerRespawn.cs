using UnityEngine;

[RequireComponent(typeof(CombatStats))]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private KeyCode respawnKey = KeyCode.R;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMotor playerMotor;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerAnimation playerAnimation;

    private CombatStats combatStats;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isDead;

    private void Awake()
    {
        combatStats = GetComponent<CombatStats>();

        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        if (playerMotor == null)
            playerMotor = GetComponent<PlayerMotor>();

        if (playerCombat == null)
            playerCombat = GetComponent<PlayerCombat>();

        if (playerAnimation == null)
            playerAnimation = GetComponent<PlayerAnimation>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnEnable()
    {
        combatStats.OnDamageTaken += HandleDamageTaken;
        combatStats.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        combatStats.OnDamageTaken -= HandleDamageTaken;
        combatStats.OnDeath -= HandleDeath;
    }

    private void Update()
    {
        if (!isDead) return;

        if (Input.GetKeyDown(respawnKey))
        {
            Respawn();
        }
    }

    private void HandleDamageTaken(int damage)
    {
        if (combatStats.IsDead) return;
        playerAnimation?.PlayTakeDamage();
    }

    private void HandleDeath()
    {
        isDead = true;

        playerAnimation?.PlayDeath();

        if (playerInput != null)
            playerInput.enabled = false;

        if (playerMotor != null)
            playerMotor.enabled = false;

        if (playerCombat != null)
            playerCombat.enabled = false;

        Debug.Log("Player morreu. Aperte R para ressurgir.");
    }

    private void Respawn()
    {
        Vector3 targetPosition;
        Quaternion targetRotation;

        if (respawnPoint != null)
        {
            targetPosition = respawnPoint.position;
            targetRotation = respawnPoint.rotation;
        }
        else
        {
            targetPosition = initialPosition;
            targetRotation = initialRotation;
        }

        if (characterController != null)
            characterController.enabled = false;

        transform.position = targetPosition;
        transform.rotation = targetRotation;

        if (characterController != null)
            characterController.enabled = true;

        combatStats.RestoreFullHealth();

        if (playerInput != null)
            playerInput.enabled = true;

        if (playerMotor != null)
            playerMotor.enabled = true;

        if (playerCombat != null)
        {
            playerCombat.enabled = true;
            playerCombat.ClearTarget();
        }

        isDead = false;

        Debug.Log("Player ressurgiu.");
    }
}