using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class EnhancedFlingableBall : MonoBehaviour
{
    [Header("Fling Settings")]
    [Tooltip("How much to scale mouse movement ? world velocity")]
    public float flingMultiplier = 1.5f;

    [Tooltip("Number of samples to average for smooth velocity")]
    public int sampleCount = 5;

    private Rigidbody rb;
    private bool isDragging;
    private Vector3 dragOffset;
    private Queue<Vector3> posSamples = new Queue<Vector3>();
    private Queue<float> timeSamples = new Queue<float>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        rb.isKinematic = true;
        isDragging = true;
        posSamples.Clear();
        timeSamples.Clear();

        // Initial offset
        Vector3 worldPos = GetMouseWorldPosition();
        dragOffset = transform.position - worldPos;

        // Seed sample queues
        posSamples.Enqueue(worldPos);
        timeSamples.Enqueue(Time.time);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 worldPos = GetMouseWorldPosition();
        transform.position = worldPos + dragOffset;

        // Record sample
        posSamples.Enqueue(worldPos);
        timeSamples.Enqueue(Time.time);

        // Maintain only last sampleCount samples
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
        rb.isKinematic = false;

        // Compute average velocity over samples
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
                Vector3 rawVel = (lastPos - firstPos) / dt;
                Vector3 flingVel = rawVel * flingMultiplier;
                flingVel.z = 0f; // keep 2D

                rb.linearVelocity = flingVel;
            }
        }

        posSamples.Clear();
        timeSamples.Clear();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screen = Input.mousePosition;
        screen.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screen);
    }
}
