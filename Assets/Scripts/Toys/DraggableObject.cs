using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class DraggableObject : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    private Camera cam;
    private Rigidbody rb;
    private bool isDragging;

    [HideInInspector]
    public bool WasReleasedByPlayer = false;

    private bool blockedByHandle = false;

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        WasReleasedByPlayer = false; // 🧼 Reset every pickup
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("RotateHandle"))
        {
            blockedByHandle = true;
            return;
        }

        offset = transform.position - GetMouseWorldPosition();
        rb.isKinematic = true;
        isDragging = true;
        blockedByHandle = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || blockedByHandle) return;
        Vector3 newPos = GetMouseWorldPosition() + offset;
        transform.position = new Vector3(newPos.x, newPos.y, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging || blockedByHandle) return;
        rb.isKinematic = false;
        isDragging = false;
        WasReleasedByPlayer = true; // ✅ Mark released
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(cam.transform.position.z);
        return cam.ScreenToWorldPoint(screenPos);
    }
}
