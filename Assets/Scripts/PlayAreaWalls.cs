using UnityEngine;

public class PlayAreaWalls : MonoBehaviour
{
    [Header("Play Area Dimensions (World Units)")]
    public float playAreaWidth = 16f;
    public float playAreaHeight = 9f;

    [Header("Wall Thickness")]
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
        if (!topWall || !bottomWall || !leftWall || !rightWall) return;

        float halfW = playAreaWidth * 0.5f;
        float halfH = playAreaHeight * 0.5f;

        // Position & scale each wall so it frames the area
        topWall.localPosition    = new Vector3(0f,   +halfH, 0f);
        topWall.localScale      = new Vector3(playAreaWidth, wallThickness, 1f);

        bottomWall.localPosition = new Vector3(0f,   -halfH, 0f);
        bottomWall.localScale    = new Vector3(playAreaWidth, wallThickness, 1f);

        leftWall.localPosition   = new Vector3(-halfW, 0f, 0f);
        leftWall.localScale      = new Vector3(wallThickness, playAreaHeight, 1f);

        rightWall.localPosition  = new Vector3(+halfW, 0f, 0f);
        rightWall.localScale     = new Vector3(wallThickness, playAreaHeight, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(playAreaWidth, playAreaHeight, 0f));
    }
}
