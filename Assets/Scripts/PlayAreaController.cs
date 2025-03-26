using UnityEngine;

public class PlayAreaController : MonoBehaviour
{
    [Header("Play Area Dimensions (World Units)")]
    [Tooltip("Desired width of the play area.")]
    public float playAreaWidth = 16f;

    [Tooltip("Desired height of the play area.")]
    public float playAreaHeight = 9f;

    // Optional: Visualize the play area in Scene view.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = Vector3.zero;
        Gizmos.DrawWireCube(center, new Vector3(playAreaWidth, playAreaHeight, 0));
    }
}
