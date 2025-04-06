using UnityEngine;

public class DeleteOnTrigger : MonoBehaviour
{
    public float velocityThreshold = 0.2f; // Adjust as needed

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Toy")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && rb.linearVelocity.magnitude < velocityThreshold)
        {
            // Only delete if the object is essentially not moving
            Destroy(other.gameObject);
        }
    }
}
