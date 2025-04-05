using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformToy : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Transform platform;    // The white bar
    public Transform pivot;       // Slightly below center
    public Transform leftHandle;
    public Transform rightHandle;

    private Camera mainCam;
    private bool dragging = false;
    private bool rotating = false;
    private Vector3 dragOffset;
    private Transform activeHandle;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        // Always keep it Z-locked to 0 (2D look)
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;

        Vector3 rot = transform.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        transform.eulerAngles = rot;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsPointerOnHandle(eventData, leftHandle))
        {
            rotating = true;
            activeHandle = leftHandle;
        }
        else if (IsPointerOnHandle(eventData, rightHandle))
        {
            rotating = true;
            activeHandle = rightHandle;
        }
        else
        {
            dragging = true;
            dragOffset = transform.position - WorldPoint(eventData.position);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rotating)
        {
            Vector3 dir = WorldPoint(eventData.position) - pivot.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            platform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (dragging)
        {
            transform.position = WorldPoint(eventData.position) + dragOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        rotating = false;
        activeHandle = null;
    }

    private Vector3 WorldPoint(Vector2 screenPos)
    {
        return mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mainCam.nearClipPlane));
    }

    private bool IsPointerOnHandle(PointerEventData eventData, Transform handle)
    {
        Vector2 screenPoint = eventData.position;
        Vector2 handleScreenPos = mainCam.WorldToScreenPoint(handle.position);
        float handleRadius = 40f; // Adjust to taste

        return Vector2.Distance(screenPoint, handleScreenPos) < handleRadius;
    }
}
