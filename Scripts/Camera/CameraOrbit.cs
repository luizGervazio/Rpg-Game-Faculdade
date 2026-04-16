using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);

    [Header("Distance")]
    public float distance = 5f;
    public float minDistance = 3f;
    public float maxDistance = 10f;
    public float zoomSpeed = 4f;

    [Header("Rotation")]
    public float rotationSpeed = 220f;
    public float minPitch = 15f;
    public float maxPitch = 70f;

    [Header("Smoothing")]
    public float positionSmoothTime = 0.05f;

    private float yaw;
    private float pitch = 25f;
    private Vector3 velocity;
    private bool rotating;

    void Start()
    {
        if (target == null)
        {
            enabled = false;
            return;
        }

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            rotating = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            rotating = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (rotating)
        {
            yaw += Input.GetAxisRaw("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxisRaw("Mouse Y") * rotationSpeed;
        }

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focusPoint = target.position + targetOffset;
        Vector3 desiredPosition = focusPoint - (rotation * Vector3.forward * distance);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            positionSmoothTime
        );

        transform.LookAt(focusPoint);
    }
}