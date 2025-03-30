using UnityEngine;
using UnityEngine.EventSystems;

public class ResizablePanel : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RectTransform targetPanel;
    public float minWidth = 100;
    public float minHeight = 100;

    private Vector2 originalSize;
    private Vector2 originalMousePosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetPanel, eventData.position, eventData.pressEventCamera, out originalMousePosition);
        originalSize = targetPanel.sizeDelta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetPanel, eventData.position, eventData.pressEventCamera, out localMousePosition);
        Vector2 offset = localMousePosition - originalMousePosition;

        float newWidth = Mathf.Max(minWidth, originalSize.x + offset.x);
        float newHeight = Mathf.Max(minHeight, originalSize.y - offset.y); // Y flips

        targetPanel.sizeDelta = new Vector2(newWidth, newHeight);
    }
}
