using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Drag Settings")]
    [Tooltip("If true, uses physics-based fling (for dynamic objects like balls). If false, uses simple transform drag (for static objects like platforms).")]
    public bool isDynamic = true;

    [Tooltip("Multiplier for fling velocity (only used if isDynamic is true).")]
    public float flingMultiplier = 1.5f;

    private Vector3 dragOffset;
    private bool dragging = false;
    private Camera mainCam;
    private Rigidbody rb;

    // For fling calculations (only for dynamic objects)
    private Vector3 lastMouseWorldPos;
    private Vector3 flingVelocity;

    void Awake()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        // For dynamic objects, switch to kinematic while dragging.
        if (isDynamic && rb != null)
        {
            rb.isKinematic = true;
        }
        Vector3 worldPos = GetMouseWorldPosition(eventData);
        dragOffset = transform.position - worldPos;
        lastMouseWorldPos = worldPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = GetMouseWorldPosition(eventData);
        transform.position = worldPos + dragOffset;

        if (isDynamic)
        {
            // Calculate fling velocity over the last frame
            flingVelocity = (worldPos - lastMouseWorldPos) / Time.deltaTime;
            lastMouseWorldPos = worldPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        if (isDynamic && rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = flingVelocity * flingMultiplier;
        }
    }

    private Vector3 GetMouseWorldPosition(PointerEventData eventData)
    {
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, Mathf.Abs(mainCam.transform.position.z));
        return mainCam.ScreenToWorldPoint(screenPos);
    }
}
