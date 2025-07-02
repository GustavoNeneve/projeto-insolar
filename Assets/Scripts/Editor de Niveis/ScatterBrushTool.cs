using UnityEngine;
using System.Collections.Generic;

public class ScatterBrushTool : BaseTool
{
    public int density = 10;
    public float radius = 1f;

    public override void OnToolActivated() { }
    public override void OnToolDeactivated() { }

    public override void OnMouseClick()
    {
        if (RaycastFromMouse(out RaycastHit hit))
        {
            ScatterAtPoint(hit.point);
        }
    }

    public override void OnMouseDrag()
    {
        if (RaycastFromMouse(out RaycastHit hit))
        {
            ScatterAtPoint(hit.point);
        }
    }

    public override void OnMouseUp() { }

    private void ScatterAtPoint(Vector3 center)
    {
        for (int i = 0; i < density; i++)
        {
            Vector2 rnd = Random.insideUnitCircle * radius;
            Vector3 pos = new Vector3(center.x + rnd.x, center.y, center.z + rnd.y);
            pos = editorManager.SnapToGrid(pos);
            var prefab = editorManager.SelectedPrefab;
            if (prefab != null)
            {
                GameObject obj = Instantiate(prefab, pos, Quaternion.Euler(0, Random.Range(0,360), 0));
                // Registrar no Undo/Redo se necessário
            }
        }
    }
}
