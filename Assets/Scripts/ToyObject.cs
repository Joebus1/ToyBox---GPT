using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ToyObject : MonoBehaviour
{
    public ToyProperties properties; // Reference your ScriptableObject here

    void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (properties != null)
        {
            rb.mass = properties.mass;
            // Assuming the collider already has a Physics Material,
            // you could adjust bounciness/friction via the material settings.
        }
    }
}
