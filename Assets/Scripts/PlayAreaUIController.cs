using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class PlayAreaUIController : MonoBehaviour
{
    public PlayAreaController playArea;
    RectTransform rt;
    Camera cam;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    void Update()
    {
        if (playArea == null || cam == null) return;

        // Convert world‑units to UI pixels
        float pixelsPerUnit = Screen.height / playArea.playAreaHeight;
        rt.sizeDelta = new Vector2(playArea.playAreaWidth * pixelsPerUnit, playArea.playAreaHeight * pixelsPerUnit);
        rt.anchoredPosition = Vector2.zero;
    }
}
