using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class FlingableBall : MonoBehaviour
{
    [Header("Fling Settings")]
    [Tooltip("How much to scale mouse movement → fling velocity")]
    public float flingMultiplier = 1.5f;

    [Tooltip("How many movement samples to average")]
    public int sampleCount = 5;

    [Header("UI References")]
    [SerializeField] private RectTransform toyBoxPanel;
    [SerializeField] private Camera uiCamera;

    private Rigidbody rb;
    private bool isDragging;
    private Vector3 dragOffset;
    private Queue<Vector3> posSamples = new();
    private Queue<float> timeSamples = new();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (uiCamera == null)
        {
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas != null)
                uiCamera = canvas.worldCamera;
        }
    }

    void OnMouseDown()
    {
        rb.isKinematic = true;
        isDragging = true;

        Vector3 worldPos = GetMouseWorldPosition();
        dragOffset = transform.position - worldPos;

        posSamples.Clear();
        timeSamples.Clear();

        posSamples.Enqueue(worldPos);
        timeSamples.Enqueue(Time.time);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 worldPos = GetMouseWorldPosition();
        transform.position = worldPos + dragOffset;

        posSamples.Enqueue(worldPos);
        timeSamples.Enqueue(Time.time);

        if (posSamples.Count > sampleCount)
        {
            posSamples.Dequeue();
            timeSamples.Dequeue();
        }
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;

        // Try to delete if dropped in ToyBoxPanel
        if (toyBoxPanel != null && uiCamera != null)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(toyBoxPanel, Input.mousePosition, uiCamera, out Vector2 localPoint))
            {
                if (toyBoxPanel.rect.Contains(localPoint))
                {
                    Debug.Log("Deleted: Ball dropped in ToyBoxPanel");
                    Destroy(gameObject);
                    return;
                }
            }
        }

        // Fling
        rb.isKinematic = false;

        if (posSamples.Count >= 2)
        {
            Vector3 firstPos = posSamples.Peek();
            float firstTime = timeSamples.Peek();

            Vector3 lastPos = default;
            float lastTime = default;

            foreach (var p in posSamples) lastPos = p;
            foreach (var t in timeSamples) lastTime = t;

            float dt = lastTime - firstTime;
            if (dt > 0f)
            {
                Vector3 rawVelocity = (lastPos - firstPos) / dt;
                Vector3 flingVelocity = rawVelocity * flingMultiplier;
                flingVelocity.z = 0f;
                rb.linearVelocity = flingVelocity;
            }
        }

        posSamples.Clear();
        timeSamples.Clear();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = uiCamera.WorldToScreenPoint(transform.position).z;
        return uiCamera.ScreenToWorldPoint(screenPos);
    }
}
