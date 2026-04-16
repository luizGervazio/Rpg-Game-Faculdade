using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float stopDistance = 0.2f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 velocity;

    public bool IsMoving { get; private set; }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;
        bool isMovingNow = false;

        if (playerInput.MoveAxis.sqrMagnitude > 0.01f)
        {
            playerInput.ClearClickTarget();

            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            moveDirection = (cameraRight * playerInput.MoveAxis.x + cameraForward * playerInput.MoveAxis.y).normalized;
            isMovingNow = true;
        }
        else if (playerInput.HasClickTarget)
        {
            Vector3 toTarget = playerInput.ClickTarget - transform.position;
            toTarget.y = 0f;

            if (toTarget.magnitude > stopDistance)
            {
                moveDirection = toTarget.normalized;
                isMovingNow = true;
            }
            else
            {
                playerInput.ClearClickTarget();
            }
        }

        if (isMovingNow)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            controller.Move(transform.forward * speed * Time.deltaTime);
        }

        ApplyGravity();

        IsMoving = isMovingNow;
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}