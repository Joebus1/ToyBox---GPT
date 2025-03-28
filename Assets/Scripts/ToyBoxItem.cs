using UnityEngine;
using UnityEngine.EventSystems;

public class ToyBoxItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public GameObject prefab;
    public RectTransform toyBoxPanel;

    private GameObject spawnedObject;
    private Camera uiCamera;

    void Awake()
    {
        // Use the canvas' camera if available; otherwise fallback to Camera.main
        uiCamera = Camera.main;
    }

    // Called when a drag begins.
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Only spawn if there isn't already a spawned object.
        if (spawnedObject == null)
            SpawnBall(eventData);
    }

    // Called while dragging.
    public void OnDrag(PointerEventData eventData)
    {
        if (spawnedObject != null)
        {
            Vector3 worldPos = GetWorldPosition(eventData);
            spawnedObject.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
        }
    }

    // Called when drag ends.
    public void OnEndDrag(PointerEventData eventData)
    {
        if (spawnedObject == null) return;

        bool droppedInToyBox = RectTransformUtility.RectangleContainsScreenPoint(
            toyBoxPanel, eventData.position, uiCamera
        );

        if (droppedInToyBox)
        {
            Destroy(spawnedObject);
        }
        else
        {
            // Release physics (if applicable)
            if (spawnedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.isKinematic = false;
        }

        spawnedObject = null;
    }

    // Called on a simple click (no drag).
    public void OnPointerClick(PointerEventData eventData)
    {
        if (spawnedObject == null)
        {
            SpawnBall(eventData);
            // Immediately release the ball to fall.
            if (spawnedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.isKinematic = false;
            spawnedObject = null;
        }
    }

    private void SpawnBall(PointerEventData eventData)
    {
        Vector3 worldPos = GetWorldPosition(eventData);
        spawnedObject = Instantiate(prefab, new Vector3(worldPos.x, worldPos.y, 0f), Quaternion.identity);

        // If using physics dragging, make it kinematic until dropped.
        if (spawnedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = true;
    }

    private Vector3 GetWorldPosition(PointerEventData eventData)
    {
        // Convert the screen position to world coordinates.
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, -uiCamera.transform.position.z);
        return uiCamera.ScreenToWorldPoint(screenPos);
    }
}
