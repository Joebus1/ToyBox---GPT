using UnityEngine;

public class PlayAreaWalls : MonoBehaviour
{
    [Header("Play Area Dimensions (World Units)")]
    public float playAreaWidth = 16f;
    public float playAreaHeight = 9f;

    [Header("Wall Thickness")]
    [Tooltip("How thick each wall should be in the X or Y dimension.")]
    public float wallThickness = 1f;

    [Header("Wall References")]
    public Transform topWall;
    public Transform bottomWall;
    public Transform leftWall;
    public Transform rightWall;

    private void OnValidate()
    {
        UpdateWalls();
    }

    private void Awake()
    {
        UpdateWalls();
    }

    public void UpdateWalls()
    {
        // Safety check
        if (!topWall || !bottomWall || !leftWall || !rightWall) return;

        float halfW = playAreaWidth * 0.5f;
        float halfH = playAreaHeight * 0.5f;

        // Top wall: centered horizontally, top edge
        topWall.localPosition = new Vector3(0f, +halfH, 0f);
        topWall.localScale = new Vector3(playAreaWidth, wallThickness, 1f);

        // Bottom wall: centered horizontally, bottom edge
        bottomWall.localPosition = new Vector3(0f, -halfH, 0f);
        bottomWall.localScale = new Vector3(playAreaWidth, wallThickness, 1f);

        // Left wall: left edge, covers full height
        leftWall.localPosition = new Vector3(-halfW, 0f, 0f);
        leftWall.localScale = new Vector3(wallThickness, playAreaHeight, 1f);

        // Right wall: right edge, covers full height
        rightWall.localPosition = new Vector3(+halfW, 0f, 0f);
        rightWall.localScale = new Vector3(wallThickness, playAreaHeight, 1f);
    }

    private void OnDrawGizmos()
    {
        // Draw a white wireframe for the play area
        Gizmos.color = Color.white;
        Vector3 center = transform.position;
        Gizmos.DrawWireCube(center, new Vector3(playAreaWidth, playAreaHeight, 0f));
    }
}
