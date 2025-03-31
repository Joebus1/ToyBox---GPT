using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformToy : MonoBehaviour
{
    public Transform body;
    private Vector3 offset;
    private bool dragging = false;
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (dragging)
        {
            Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = transform.position.z;
            transform.position = mouse + offset;
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = transform.position.z;
            offset = transform.position - mouse;
            dragging = true;
        }
    }

    void OnMouseUp()
    {
        dragging = false;
    }
}
