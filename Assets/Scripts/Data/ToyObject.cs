using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class ToyObject : MonoBehaviour
{
    public ToyProperties properties;

    private void Awake()
    {
        if (properties != null)
        {
            ApplyProperties();
        }
    }

    public void ApplyProperties()
    {
        if (properties == null) return;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.mass = properties.mass;
            rb.linearDamping = 0f; // Customize if needed
            rb.angularDamping = 0.05f; // Customize if needed
            rb.gravityScale = 1f;
        }

        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            PhysicsMaterial2D material = new PhysicsMaterial2D("ToyMaterial")
            {
                bounciness = properties.bounciness,
                friction = properties.friction
            };
            collider.sharedMaterial = material;
        }
        else
        {
            Debug.LogWarning($"{name} has no Collider2D to apply physics material to.");
        }
    }
}
