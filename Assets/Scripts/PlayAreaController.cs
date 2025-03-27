using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayAreaController : MonoBehaviour
{
    [Header("Play Area Dimensions (World Units)")]
    public float playAreaWidth = 16f;
    public float playAreaHeight = 9f;

    private BoxCollider boundaryCollider;

    void Awake()
    {
        boundaryCollider = GetComponent<BoxCollider>();
        UpdateBoundary();
    }

    void OnValidate()
    {
        if (boundaryCollider == null)
            boundaryCollider = GetComponent<BoxCollider>();
        UpdateBoundary();
    }

    void UpdateBoundary()
    {
        // The boundary is centered at (0,0) and its size equals the play area dimensions.
        boundaryCollider.center = Vector3.zero;
        boundaryCollider.size = new Vector3(playAreaWidth, playAreaHeight, 1f);
    }

    // Visualize the play area in Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(playAreaWidth, playAreaHeight, 0));
    }
}
