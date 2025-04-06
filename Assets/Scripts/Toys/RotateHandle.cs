using UnityEngine;
using UnityEngine.EventSystems;

public class RotateHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Tooltip("Reference to the platform's Body that should rotate.")]
    public Transform platform;
    [Tooltip("Reference to the Pivot object around which rotation occurs.")]
    public Transform pivot;

    private bool rotating = false;
    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rotating = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!rotating || platform == null || pivot == null) return;

        Vector3 mouseWorld = GetMouseWorldPosition(eventData);
        Vector2 direction = mouseWorld - pivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        platform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rotating = false;
    }

    private Vector3 GetMouseWorldPosition(PointerEventData eventData)
    {
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, Mathf.Abs(mainCam.transform.position.z));
        return mainCam.ScreenToWorldPoint(screenPos);
    }
}
