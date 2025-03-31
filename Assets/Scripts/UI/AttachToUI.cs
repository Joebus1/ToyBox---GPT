using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AttachToUI : MonoBehaviour
{
    public RectTransform toyBoxPanel;
    public Camera uiCamera;

    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void LateUpdate()
    {
        if (toyBoxPanel == null || uiCamera == null) return;

        // Match Position
        Vector3[] corners = new Vector3[4];
        toyBoxPanel.GetWorldCorners(corners);
        Vector3 center = (corners[0] + corners[2]) / 2f;
        transform.position = new Vector3(center.x, center.y, 0f);

        // Match Size
        float width = Vector3.Distance(corners[0], corners[3]);
        float height = Vector3.Distance(corners[0], corners[1]);
        boxCollider.size = new Vector3(width, height, 1f);
        boxCollider.center = Vector3.zero;
    }
}
