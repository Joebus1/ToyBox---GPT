using UnityEngine;

public class PlatformHandle : MonoBehaviour
{
    public Transform platformRoot;
    private Camera cam;
    private bool rotating = false;
    private Vector3 center;

    void Start()
    {
        cam = Camera.main;
        center = platformRoot.position;
    }

    void OnMouseDown()
    {
        rotating = true;
    }

    void OnMouseUp()
    {
        rotating = false;
    }

    void Update()
    {
        if (rotating)
        {
            Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mouse - center;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            platformRoot.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
