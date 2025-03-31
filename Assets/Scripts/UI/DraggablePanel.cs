using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggablePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private RectTransform panelRectTransform;
    private RectTransform parentRectTransform;

    void Awake()
    {
        panelRectTransform = transform as RectTransform;
        parentRectTransform = panelRectTransform.parent as RectTransform;
    }

    public void OnBeginDrag(PointerEventData data)
    {
        // Record current panel position and mouse offset
        originalPanelLocalPosition = panelRectTransform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition
        );
    }

    public void OnDrag(PointerEventData data)
    {
        if (panelRectTransform == null || parentRectTransform == null) return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform, data.position, data.pressEventCamera, out localPointerPosition
        ))
        {
            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
            panelRectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
            ClampToWindow();
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        // Nothing special on end drag for now
    }

    // Inside DraggablePanel.cs, using parentRectTransform as before
    void ClampToWindow()
    {
        Vector3 pos = panelRectTransform.localPosition;

        Vector3 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
        Vector3 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;

        pos.x = Mathf.Clamp(pos.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(pos.y, minPosition.y, maxPosition.y);

        panelRectTransform.localPosition = pos;
    }
}
