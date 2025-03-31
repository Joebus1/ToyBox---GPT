using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformToy : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Transform platform; // Should be "Body"
    public Transform pivot;    // Should be "Pivot"

    public void OnPointerDown(PointerEventData eventData)
    {
        // Nothing needed here yet
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (platform == null || pivot == null) return;

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 dir = mouseWorld - (Vector2)pivot.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        platform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
