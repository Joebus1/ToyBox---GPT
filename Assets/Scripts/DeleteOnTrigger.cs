using UnityEngine;

public class DeleteOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Toy"))
        {
            Debug.Log("Toy entered delete zone — destroyed.");
            Destroy(other.gameObject);
        }
    }
}
