using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Tooltip("Reference to the PlayAreaWalls.")]
    public PlayAreaWalls playAreaWalls;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        UpdateCameraSize();
    }

    void OnValidate()
    {
        if (cam == null) cam = GetComponent<Camera>();
        UpdateCameraSize();
    }

    void UpdateCameraSize()
    {
        if (playAreaWalls != null && cam != null)
        {
            // Orthographic camera covers exactly the playAreaHeight top-to-bottom
            cam.orthographicSize = playAreaWalls.playAreaHeight * 0.5f;
        }
    }
}
