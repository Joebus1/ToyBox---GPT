using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformRotator : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Transform body;
    [SerializeField] private Transform pivot;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Required for IPointerDownHandler, but we don’t need to do anything here.
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (body == null || pivot == null || cam == null) return;

        Vector3 screenPoint = new(eventData.position.x, eventData.position.y, Mathf.Abs(cam.transform.position.z));
        Vector3 worldPoint = cam.ScreenToWorldPoint(screenPoint);

        Vector3 dir = worldPoint - pivot.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Only rotate around Z axis (since we’re faking 2D)
        body.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
