using UnityEngine;
using UnityEngine.EventSystems;

public class ToyBoxItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Prefab Settings")]
    [Tooltip("The 3D ball prefab to spawn into the play area.")]
    public GameObject prefab;

    [Header("UI Settings")]
    [Tooltip("The RectTransform of the ToyBoxPanel (used to check if a ball is dragged back in).")]
    public RectTransform toyBoxPanel;

    private GameObject spawnedObject;
    private Camera mainCamera;
    private bool isDragging = false;

    void Awake()
    {
        // Cache the main camera (make sure your Main Camera has the tag "MainCamera")
        mainCamera = Camera.main;
    }

    // Called when a drag begins.
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        SpawnBall(eventData);
    }

    // Called during dragging; update the spawned ball’s position.
    public void OnDrag(PointerEventData eventData)
    {
        if (spawnedObject != null)
        {
            Vector3 worldPos = GetWorldPosition(eventData);
            // We keep z at 0 for our 2D play area.
            spawnedObject.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
        }
    }

    // Called when the drag ends.
    public void OnEndDrag(PointerEventData eventData)
    {
        if (spawnedObject == null)
            return;

        // Check if the ball is dropped inside the toybox.
        bool droppedInToyBox = RectTransformUtility.RectangleContainsScreenPoint(toyBoxPanel, eventData.position, mainCamera);

        if (droppedInToyBox)
        {
            // Delete the ball.
            Destroy(spawnedObject);
        }
        else if (spawnedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // Release the ball to physics.
            rb.isKinematic = false;
        }

        spawnedObject = null;
        isDragging = false;
    }

    // Called when the user simply clicks the UI element.
    public void OnPointerClick(PointerEventData eventData)
    {
        // Only spawn if we are not in the middle of a drag.
        if (!isDragging)
        {
            SpawnBall(eventData);
            if (spawnedObject != null && spawnedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                // Immediately release physics so the ball falls.
                rb.isKinematic = false;
            }
            spawnedObject = null;
        }
    }

    // Helper method to spawn a new ball at the pointer position.
    private void SpawnBall(PointerEventData eventData)
    {
        Vector3 worldPos = GetWorldPosition(eventData);
        spawnedObject = Instantiate(prefab, new Vector3(worldPos.x, worldPos.y, 0f), Quaternion.identity);

        // Temporarily disable physics so the ball follows the pointer.
        if (spawnedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
    }

    // Converts screen point to world position.
    private Vector3 GetWorldPosition(PointerEventData eventData)
    {
        // Since our Main Camera is orthographic, we use its z distance.
        return mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, -mainCamera.transform.position.z));
    }
}
