using UnityEngine;

public class DeleteOnTrigger : MonoBehaviour
{
    public float velocityThreshold = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Toy")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && rb.linearVelocity.magnitude <= velocityThreshold)
        {
            Destroy(other.gameObject);
        }
    }
}
