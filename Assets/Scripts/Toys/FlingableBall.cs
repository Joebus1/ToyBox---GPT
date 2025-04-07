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

    [HideInInspector] public bool wasReleasedByPlayer = false;

    private Rigidbody rb;
    private bool isDragging;
    private Vector3 dragOffset;
    private readonly Queue<Vector3> posSamples = new();
    private readonly Queue<float> timeSamples = new();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wasReleasedByPlayer = false; // ✅ Set this just to be safe
    }

    void OnMouseDown()
    {
        rb.isKinematic = true;
        isDragging = true;
        wasReleasedByPlayer = false;

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

    [System.Obsolete]
    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;

        rb.isKinematic = false;

        // Determine fling velocity
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
                rb.velocity = flingVelocity;
            }
        }

        wasReleasedByPlayer = true;
        posSamples.Clear();
        timeSamples.Clear();
    }


    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = 10f; // Arbitrary Z distance from camera
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
