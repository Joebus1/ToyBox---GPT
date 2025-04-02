using UnityEngine;

public class AdjustablePlatform : MonoBehaviour
{
    public Transform leftHandle;
    public Transform rightHandle;
    public Transform body;

    private bool isDraggingPlatform = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private Vector3 offset;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = GetMouseWorldPosition();

            if (IsNearHandle(leftHandle, mouseWorld))
            {
                isRotatingLeft = true;
            }
            else if (IsNearHandle(rightHandle, mouseWorld))
            {
                isRotatingRight = true;
            }
            else if (IsOverBody(mouseWorld))
            {
                isDraggingPlatform = true;
                offset = transform.position - mouseWorld;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraggingPlatform = false;
            isRotatingLeft = false;
            isRotatingRight = false;
        }

        if (isDraggingPlatform)
        {
            Vector3 newPos = GetMouseWorldPosition() + offset;
            newPos.z = 0;
            transform.position = newPos;
        }

        if (isRotatingLeft || isRotatingRight)
        {
            Vector3 pivot = isRotatingLeft ? leftHandle.position : rightHandle.position;
            Vector3 direction = GetMouseWorldPosition() - pivot;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            body.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    bool IsNearHandle(Transform handle, Vector3 pos)
    {
        return Vector3.Distance(handle.position, pos) < 0.5f;
    }

    bool IsOverBody(Vector3 pos)
    {
        Collider col = body.GetComponent<Collider>();
        return col != null && col.bounds.Contains(pos);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 screen = Input.mousePosition;
        screen.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screen);
    }
}
