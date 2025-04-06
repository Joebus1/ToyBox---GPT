using UnityEngine;

public class DeleteOnTrigger : MonoBehaviour
{
    public float velocityThreshold = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Toy")) return;

        DraggableObject draggable = other.GetComponent<DraggableObject>();
        Rigidbody rb = other.attachedRigidbody;

        if (draggable != null && rb != null)
        {
            if (draggable.WasReleasedByPlayer && rb.linearVelocity.magnitude <= velocityThreshold)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
