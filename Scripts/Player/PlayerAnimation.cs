using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerAnimation : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int AttackLeftHash = Animator.StringToHash("AttackLeft");
    private static readonly int AttackRightHash = Animator.StringToHash("AttackRight");
    private static readonly int TakeDamageHash = Animator.StringToHash("TakeDamage");
    private static readonly int DeathHash = Animator.StringToHash("Death");

    private Animator animator;
    private PlayerMotor playerMotor;
    private bool useLeftAttack = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMotor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        animator.SetBool(IsMovingHash, playerMotor != null && playerMotor.IsMoving);
    }

    public void PlayAttack()
    {
        if (useLeftAttack)
            animator.SetTrigger(AttackLeftHash);
        else
            animator.SetTrigger(AttackRightHash);

        useLeftAttack = !useLeftAttack;
    }

    public void PlayTakeDamage()
    {
        animator.SetTrigger(TakeDamageHash);
    }

    public void PlayDeath()
    {
        animator.SetTrigger(DeathHash);
    }
}