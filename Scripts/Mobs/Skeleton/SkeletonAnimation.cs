using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SkeletonAnimation : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int TakeDamageHash = Animator.StringToHash("TakeDamage");
    private static readonly int DeathHash = Animator.StringToHash("Death");
    private static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    private static readonly int Attack02Hash = Animator.StringToHash("Attack02");

    private Animator animator;
    private bool useFirstAttack = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMoving(bool isMoving)
    {
        animator.SetBool(IsMovingHash, isMoving);
    }

    public void PlayTakeDamage()
    {
        animator.SetTrigger(TakeDamageHash);
    }

    public void PlayDeath()
    {
        animator.SetTrigger(DeathHash);
    }

    public void PlayAttack()
    {
        if (useFirstAttack)
        {
            animator.SetTrigger(Attack01Hash);
        }
        else
        {
            animator.SetTrigger(Attack02Hash);
        }

        useFirstAttack = !useFirstAttack;
    }
}