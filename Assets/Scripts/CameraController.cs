using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Tooltip("Reference to the Play Area Controller.")]
    public PlayAreaController playAreaController;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        UpdateCameraSize();
    }

    // Call this whenever you change the play area dimensions.
    public void UpdateCameraSize()
    {
        if (playAreaController != null)
        {
            // For an orthographic camera, size is half the vertical size.
            cam.orthographicSize = playAreaController.playAreaHeight / 2f;
        }
    }

#if UNITY_EDITOR
    // Update in editor when values change.
    private void OnValidate()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
        UpdateCameraSize();
    }
#endif
}
