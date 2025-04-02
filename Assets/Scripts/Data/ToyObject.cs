using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class ToyObject : MonoBehaviour
{
    public ToyProperties properties;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

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

        // Mass & Rigidbody2D physics
        rb.mass = properties.mass;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.gravityScale = 1f;

        // Material assignment
        var mat = new PhysicsMaterial2D("RuntimeToyMaterial")
        {
            bounciness = properties.bounciness,
            friction = properties.friction
        };

        col.sharedMaterial = mat;
    }

    public void ResetMaterial()
    {
        if (col != null)
        {
            col.sharedMaterial = null;
        }
    }
}
