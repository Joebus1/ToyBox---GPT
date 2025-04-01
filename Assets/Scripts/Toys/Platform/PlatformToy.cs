using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformToy : MonoBehaviour
{
    public Transform platform; // The visual/body object that rotates
    public Transform pivot;    // The rotation pivot point
    public Transform leftHandle;
    public Transform rightHandle;

    private Camera cam;
    private bool isDraggingPlatform = false;
    private bool isRotating = false;
    private Vector3 dragOffset;
    private Transform draggedHandle;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);
            if (hit != null)
            {
                if (hit.transform == leftHandle || hit.transform == rightHandle)
                {
                    draggedHandle = hit.transform;
                    isRotating = true;
                }
                else if (hit.transform == transform)
                {
                    isDraggingPlatform = true;
                    dragOffset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDraggingPlatform = false;
            isRotating = false;
            draggedHandle = null;
        }

        if (isDraggingPlatform)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos + dragOffset;
        }
        else if (isRotating && draggedHandle != null)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - pivot.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            platform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
