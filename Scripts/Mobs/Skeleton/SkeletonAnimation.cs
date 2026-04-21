using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SkeletonAnimation : MonoBehaviour
{
    private static readonly int TakeDamageHash = Animator.StringToHash("TakeDamage");
    private static readonly int DeathHash = Animator.StringToHash("Death");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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