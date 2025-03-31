using UnityEngine;

public class DeleteOnTrigger : MonoBehaviour
{
    public float velocityThreshold = 0.1f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Toy")) return;

        FlingableBall flingable = other.GetComponent<FlingableBall>();
        Rigidbody rb = other.attachedRigidbody;

        if (flingable != null && flingable.wasReleasedByPlayer && rb != null)
        {
            // Only delete if it's basically not moving
            if (rb.linearVelocity.magnitude < velocityThreshold)
            {
                flingable.wasReleasedByPlayer = false;
                Destroy(other.gameObject);
            }
        }
    }
}
