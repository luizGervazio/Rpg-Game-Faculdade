using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMotor motor;

    void Awake()
    {
        animator = GetComponent<Animator>();
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        animator.SetBool("IsMoving", motor.IsMoving);
    }
}