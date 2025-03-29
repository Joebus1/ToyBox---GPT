using UnityEngine;
using UnityEngine.EventSystems;

public class ToyBoxItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Prefab Settings")]
    public GameObject prefab;

    [Header("UI References")]
    [SerializeField] private RectTransform toyBoxPanel;
    [SerializeField] private Camera uiCamera;

    private GameObject tempObject;
    private bool dragging;

    private void Awake()
    {
        if (uiCamera == null)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
                uiCamera = canvas.worldCamera;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 worldPos = GetWorldPosition(eventData);
        GameObject ball = Instantiate(prefab, new Vector3(worldPos.x, worldPos.y, 0f), Quaternion.identity);

        if (ball.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = false;

        ball.AddComponent<ToyObject>(); // Tag it for ClearAll
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragging) return;

        Vector3 worldPos = GetWorldPosition(eventData);
        tempObject = Instantiate(prefab, new Vector3(worldPos.x, worldPos.y, 0f), Quaternion.identity);

        if (tempObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = true;

        tempObject.AddComponent<ToyObject>();
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
        if (tempObject == null)
        {
            dragging = false;
            return;
        }

        Vector2 localPoint;
        bool droppedInToyBox = false;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(toyBoxPanel, eventData.position, uiCamera, out localPoint))
        {
            if (toyBoxPanel.rect.Contains(localPoint))
                droppedInToyBox = true;
        }

        if (droppedInToyBox)
        {
            Debug.Log("Dropped in ToyBox — destroyed.");
            Destroy(tempObject);
        }
        else
        {
            if (tempObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.isKinematic = false;
        }

        tempObject = null;
        dragging = false;
    }

    private Vector3 GetWorldPosition(PointerEventData eventData)
    {
        Vector3 screenPos = new(eventData.position.x, eventData.position.y, -uiCamera.transform.position.z);
        return uiCamera.ScreenToWorldPoint(screenPos);
    }
}
