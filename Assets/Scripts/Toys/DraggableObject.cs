using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Drag Mode")]
    [Tooltip("If true, object uses physics-based fling (for dynamic objects like balls). If false, uses simple transform drag (for static objects like platforms).")]
    public bool isDynamic = true;

    [Tooltip("Fling multiplier (only used if isDynamic is true).")]
    public float flingMultiplier = 1.5f;

    private Vector3 dragOffset;
    private bool dragging = false;
    private Camera mainCam;
    private Rigidbody rb;

    // For dynamic fling calculations
    private Vector3 lastMouseWorldPos;
    private Vector3 calculatedFlingVelocity;

    void Awake()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        // For dynamic toys, switch to kinematic while dragging.
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
        if (!dragging) return;

        Vector3 worldPos = GetMouseWorldPosition(eventData);
        transform.position = worldPos + dragOffset;

        if (isDynamic && rb != null)
        {
            // Calculate fling velocity based on movement
            calculatedFlingVelocity = (worldPos - lastMouseWorldPos) / Time.deltaTime;
            lastMouseWorldPos = worldPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        if (isDynamic && rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = calculatedFlingVelocity * flingMultiplier;
        }
    }

    private Vector3 GetMouseWorldPosition(PointerEventData eventData)
    {
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, Mathf.Abs(mainCam.transform.position.z));
        return mainCam.ScreenToWorldPoint(screenPos);
    }
}
