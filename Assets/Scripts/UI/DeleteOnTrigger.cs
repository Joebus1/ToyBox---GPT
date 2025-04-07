using UnityEngine;
using System.Collections.Generic;

public class DeleteOnTrigger : MonoBehaviour
{
    private readonly HashSet<GameObject> insideObjects = new();

    private void OnTriggerEnter(Collider other)
    {
        insideObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        insideObjects.Remove(other.gameObject);

        // Reset flag on exit
        if (other.TryGetComponent<FlingableBall>(out var flingable))
        {
            flingable.wasReleasedByPlayer = false;
        }
        else if (other.TryGetComponent<DraggableObject>(out var draggable))
        {
            draggable.WasReleasedByPlayer = false;
        }
    }

    private void Update()
    {
        foreach (var obj in insideObjects)
        {
            if (obj == null) continue;

            if (obj.TryGetComponent<FlingableBall>(out var flingable))
            {
                if (flingable.wasReleasedByPlayer)
                {
                    Destroy(obj);
                    break; // Modify collection, exit loop
                }
            }
            else if (obj.TryGetComponent<DraggableObject>(out var draggable))
            {
                if (draggable.WasReleasedByPlayer)
                {
                    Destroy(obj);
                    break;
                }
            }
        }
    }
}
