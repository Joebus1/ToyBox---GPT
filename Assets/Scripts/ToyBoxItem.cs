using UnityEngine;
using UnityEngine.EventSystems;

public class ToyBoxItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public GameObject prefab;

    private GameObject tempObject;
    private bool dragging;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 worldPos = GetWorldPosition(eventData);
        GameObject spawned = Instantiate(prefab, new Vector3(worldPos.x, worldPos.y, 0f), Quaternion.identity);

        if (spawned.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragging) return;

        Vector3 worldPos = GetWorldPosition(eventData);
        tempObject = Instantiate(prefab, new Vector3(worldPos.x, worldPos.y, 0f), Quaternion.identity);

        if (tempObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = true;

        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (tempObject != null)
        {
            Vector3 worldPos = GetWorldPosition(eventData);
            tempObject.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (tempObject == null) return;

        if (tempObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = false;

        tempObject = null;
        dragging = false;
    }

    private Vector3 GetWorldPosition(PointerEventData eventData)
    {
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, 10f);
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
