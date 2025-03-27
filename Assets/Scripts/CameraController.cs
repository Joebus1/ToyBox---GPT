using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Tooltip("Reference to the PlayAreaController.")]
    public PlayAreaController playArea;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        UpdateCameraSize();
    }

    void Start()
    {
        UpdateCameraSize();
    }

    public void UpdateCameraSize()
    {
        if (playArea != null && cam != null)
        {
            // Set the orthographic size so that the vertical view equals the play area height.
            cam.orthographicSize = playArea.playAreaHeight / 2f;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
        UpdateCameraSize();
    }
#endif
}
