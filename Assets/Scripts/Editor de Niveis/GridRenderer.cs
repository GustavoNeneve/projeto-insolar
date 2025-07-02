using UnityEngine;

[ExecuteAlways]
public class GridRenderer : MonoBehaviour
{
    public float cellSize = 1f;
    public int gridExtent = 20;
    public Color gridColor = Color.green;
    public bool showGrid = true;

    private void OnDrawGizmos()
    {
        if (!showGrid) return;
        Gizmos.color = gridColor;
        for (int x = -gridExtent; x <= gridExtent; x++)
        {
            for (int z = -gridExtent; z <= gridExtent; z++)
            {
                Vector3 start = new Vector3(x * cellSize, 0, -gridExtent * cellSize);
                Vector3 end = new Vector3(x * cellSize, 0, gridExtent * cellSize);
                Gizmos.DrawLine(start, end);
                start = new Vector3(-gridExtent * cellSize, 0, z * cellSize);
                end = new Vector3(gridExtent * cellSize, 0, z * cellSize);
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
