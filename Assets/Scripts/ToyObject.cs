using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ToyObject : MonoBehaviour
{
    public ToyProperties properties;

    void Awake()
    {
        ApplyProperties();
    }

    void ApplyProperties()
    {
        if (properties == null) return;

        // Set mass
        var rb = GetComponent<Rigidbody>();
        rb.mass = properties.mass;
        rb.angularDamping = 0.05f;

        // Create and apply dynamic physics material
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            var mat = new PhysicsMaterial
            {
                bounciness = properties.bounciness,
                dynamicFriction = properties.friction,
                staticFriction = properties.friction,
                bounceCombine = PhysicsMaterialCombine.Multiply,
                frictionCombine = PhysicsMaterialCombine.Average
            };

            collider.material = mat;
        }
    }
}
