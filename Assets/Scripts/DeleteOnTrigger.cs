using UnityEngine;

public class DeleteOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Toy"))
        {
            FlingableBall flingScript = other.GetComponent<FlingableBall>();
            if (flingScript != null && flingScript.wasReleasedByPlayer)
            {
                Debug.Log("Ball entered DeleteZone and was released. Deleting...");
                Destroy(other.gameObject);
            }
        }
    }
}
