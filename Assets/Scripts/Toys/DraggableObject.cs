using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableObject : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private Rigidbody rb;

    // 🔁 This is the important property needed for DeleteOnTrigger.cs
    public bool WasReleasedByPlayer { get; private set; }

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        WasReleasedByPlayer = false;
        offset = transform.position - GetMouseWorldPosition();
        rb.isKinematic = true;
    }

    void OnMouseDrag()
    {
        Vector3 target = GetMouseWorldPosition() + offset;
        target.z = 0f;
        transform.position = target;
    }

    void OnMouseUp()
    {
        WasReleasedByPlayer = true;
        rb.isKinematic = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screen = Input.mousePosition;
        screen.z = Mathf.Abs(cam.transform.position.z);
        return cam.ScreenToWorldPoint(screen);
    }
}
