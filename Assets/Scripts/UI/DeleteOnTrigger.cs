using UnityEngine;
using System.Collections.Generic;

public class DeleteOnTrigger : MonoBehaviour
{
    private readonly HashSet<GameObject> insideObjects = new();
    public float velocityThreshold = 0.2f; // Prevent fling deletion

    private void OnTriggerEnter(Collider other)
    {
        insideObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        insideObjects.Remove(other.gameObject);

        // Reset flag on exit
        if (other.TryGetComponent<ToyDraggable>(out var combo))
        {
            combo.wasReleasedByPlayer = false;
        }
    }

    private void Update()
    {
        foreach (var obj in insideObjects)
        {
            if (obj == null) continue;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (!rb) continue;

            float velocity = rb.linearVelocity.magnitude;
            if (velocity > velocityThreshold) continue;

            if (obj.TryGetComponent<ToyDraggable>(out var combo))
            {
                if (combo.wasReleasedByPlayer)
                {
                    Destroy(obj);
                    break; // Safe to exit after collection modification
                }
            }
        }
    }
}
