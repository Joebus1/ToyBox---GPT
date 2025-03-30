using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ResponsiveGrid : MonoBehaviour
{
    public int minColumns = 1;
    public int maxColumns = 10;
    public Vector2 cellSize = new Vector2(64, 64);
    public Vector2 spacing = new Vector2(10, 10);

    private RectTransform rectTransform;
    private GridLayoutGroup grid;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
    }

    void Update()
    {
        float width = rectTransform.rect.width;

        float totalCellWidth = cellSize.x + spacing.x;
        int columnCount = Mathf.FloorToInt((width + spacing.x) / totalCellWidth);
        columnCount = Mathf.Clamp(columnCount, minColumns, maxColumns);

        float newCellWidth = (width - spacing.x * (columnCount - 1)) / columnCount;

        grid.cellSize = new Vector2(newCellWidth, newCellWidth); // Keep square buttons
        grid.spacing = spacing;
    }
}
