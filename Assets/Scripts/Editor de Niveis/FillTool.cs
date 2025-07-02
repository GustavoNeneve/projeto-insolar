using UnityEngine;
using System.Collections.Generic;

public class FillTool : BaseTool
{
    private Vector3 startPoint, endPoint;
    private bool isDefiningArea = false;

    public override void OnToolActivated() { }
    public override void OnToolDeactivated() { }

    public override void OnMouseClick()
    {
        if (!isDefiningArea)
        {
            if (RaycastFromMouse(out RaycastHit hit))
            {
                startPoint = hit.point;
                isDefiningArea = true;
                // Mostrar feedback visual
            }
        }
        else
        {
            if (RaycastFromMouse(out RaycastHit hit))
            {
                endPoint = hit.point;
                FillArea(startPoint, endPoint);
                isDefiningArea = false;
            }
        }
    }

    public override void OnMouseDrag() { }
    public override void OnMouseUp() { }

    private void FillArea(Vector3 from, Vector3 to)
    {
        // Calcular bounds, preencher com prefabs, Snap to Grid, registrar Undo/Redo
    }
}
