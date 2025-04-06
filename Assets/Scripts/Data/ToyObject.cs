using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class ToyObject : MonoBehaviour
{
    public ToyProperties properties;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (properties != null)
        {
            ApplyProperties();
        }
        else
        {
            Debug.LogWarning($"ToyObject on {gameObject.name} has no ToyProperties assigned.");
        }
    }

    public void ApplyProperties()
    {
        if (properties == null || rb == null || col == null)
        {
            Debug.LogWarning($"Missing reference on {gameObject.name} in ApplyProperties.");
            return;
        }

        // Apply mass
        rb.mass = properties.mass;
        rb.useGravity = true;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;

        // Apply physics material dynamically
        PhysicsMaterial material = new("ToyMaterial")
        {
            bounciness = properties.bounciness,
            dynamicFriction = properties.friction,
            staticFriction = properties.friction,
            bounceCombine = PhysicsMaterialCombine.Maximum,
            frictionCombine = PhysicsMaterialCombine.Average
        };

        col.material = material;
    }

    public void ResetMaterial()
    {
        if (col != null)
        {
            col.material = null;
        }
    }
}
