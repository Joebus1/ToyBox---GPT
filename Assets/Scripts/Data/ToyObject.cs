using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
[DisallowMultipleComponent]
public class ToyObject : MonoBehaviour
{
    public ToyProperties properties;

    private void Awake()
    {
        ApplyProperties();
    }

    public void ApplyProperties()
    {
        if (properties == null)
        {
            Debug.LogWarning($"ToyObject on {gameObject.name} has no ToyProperties assigned.");
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        rb.mass = properties.mass;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.useGravity = true;

        // Create a runtime PhysicsMaterial and assign it.
        PhysicsMaterial mat = new PhysicsMaterial("RuntimeToyMaterial")
        {
            bounciness = properties.bounciness,
            dynamicFriction = properties.friction,
            staticFriction = properties.friction,
            bounceCombine = PhysicsMaterialCombine.Maximum,
            frictionCombine = PhysicsMaterialCombine.Average
        };

        col.material = mat;
    }
}
