using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask = ~0;

    public Vector2 MoveAxis { get; private set; }
    public bool HasClickTarget { get; private set; }
    public Vector3 ClickTarget { get; private set; }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        MoveAxis = new Vector2(x, z).normalized;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundMask))
            {
                ClickTarget = hit.point;
                HasClickTarget = true;
            }
        }
    }

    public void SetClickTarget(Vector3 targetPosition)
    {
        ClickTarget = targetPosition;
        HasClickTarget = true;
    }

    public void ClearClickTarget()
    {
        HasClickTarget = false;
    }
}