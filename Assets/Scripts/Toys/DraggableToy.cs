using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableToy : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCam;
    private Rigidbody rb;

    void Awake()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        rb.isKinematic = true;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        Vector3 targetPos = GetMouseWorldPos() + offset;
        targetPos.z = 0f;
        transform.position = targetPos;
    }

    void OnMouseUp()
    {
        rb.isKinematic = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 screen = Input.mousePosition;
        screen.z = Mathf.Abs(mainCam.transform.position.z);
        return mainCam.ScreenToWorldPoint(screen);
    }
}
