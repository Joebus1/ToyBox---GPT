using UnityEngine;

[DisallowMultipleComponent]
public class PlatformToy : MonoBehaviour
{
    public Transform platform;   // Main visual object
    public Transform pivot;      // Center pivot point
    public Transform leftHandle; // Left red tip
    public Transform rightHandle;// Right red tip

    private bool draggingPlatform = false;
    private bool draggingHandle = false;
    private Vector3 dragOffset;
    private Transform draggedHandle;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == leftHandle || hit.transform == rightHandle)
                {
                    draggingHandle = true;
                    draggedHandle = hit.transform;
                }
                else if (hit.transform == platform)
                {
                    draggingPlatform = true;
                    dragOffset = platform.position - hit.point;
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (draggingHandle && draggedHandle != null)
                {
                    Vector3 direction = hit.point - pivot.position;
                    direction.z = 0; // lock to 2D plane
                    platform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                }
                else if (draggingPlatform)
                {
                    Vector3 newPos = hit.point + dragOffset;
                    newPos.z = platform.position.z;
                    platform.position = newPos;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            draggingHandle = false;
            draggingPlatform = false;
        }
    }
}
