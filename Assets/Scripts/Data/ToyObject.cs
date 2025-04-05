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

        var rb = GetComponent<Rigidbody>();
        rb.mass = properties.mass;
        rb.angularDamping = 0.05f;

        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            var mat = new PhysicsMaterial
            {
                bounciness = properties.bounciness,
                dynamicFriction = properties.friction,
                staticFriction = properties.friction,
                bounceCombine = PhysicsMaterialCombine.Maximum,
                frictionCombine = PhysicsMaterialCombine.Minimum
            };
            collider.material = mat;
        }
    }
}
