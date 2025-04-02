using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformToy : MonoBehaviour
{
    public Transform platform;      // Usually the "Body"
    public Transform pivot;         // Custom pivot object
    public Transform leftHandle;
    public Transform rightHandle;

    private bool isDragging = false;
    private bool rotating = false;
    private Vector3 offset;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Make platform non-physical (static) but still interactable
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = GetMouseWorldPosition();

            if (IsPointerOverHandle(leftHandle, mouseWorld) || IsPointerOverHandle(rightHandle, mouseWorld))
            {
                rotating = true;
            }
            else if (IsPointerOverPlatform(mouseWorld))
            {
                isDragging = true;
                offset = transform.position - mouseWorld;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            rotating = false;
        }

        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPosition() + offset;
            transform.position = targetPos;
        }

        if (rotating)
        {
            Vector3 dir = GetMouseWorldPosition() - pivot.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            platform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(mainCamera.transform.position.z); // if using orthographic, set manually
        return mainCamera.ScreenToWorldPoint(mouse);
    }

    bool IsPointerOverHandle(Transform handle, Vector3 mousePos)
    {
        float dist = Vector3.Distance(mousePos, handle.position);
        return dist < 0.4f; // adjust if needed
    }

    bool IsPointerOverPlatform(Vector3 mousePos)
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return false;
        return col.bounds.Contains(mousePos);
    }
}
