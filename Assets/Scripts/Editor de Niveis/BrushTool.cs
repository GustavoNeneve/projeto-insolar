using UnityEngine;
using System.Collections.Generic;

public class BrushTool : BaseTool
{
    public float minSpacing = 1f;
    private Vector3 lastPosition;

    public override void OnToolActivated()
    {
        // Ativar lógica do pincel
    }

    public override void OnToolDeactivated()
    {
        // Limpar estado
    }

    public override void OnMouseClick()
    {
        PlacePrefabAtMouse();
    }

    public override void OnMouseDrag()
    {
        if (Vector3.Distance(lastPosition, GetMouseWorldPosition()) > minSpacing)
        {
            PlacePrefabAtMouse();
        }
    }

    public override void OnMouseUp() { }

    private void PlacePrefabAtMouse()
    {
        if (RaycastFromMouse(out RaycastHit hit))
        {
            Vector3 pos = editorManager.SnapToGrid(hit.point);
            GameObject obj = Instantiate(editorManager.SelectedPrefab, pos, Quaternion.identity);
            lastPosition = pos;
            // Aplicar rotação/escala, registrar no Undo/Redo
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        if (RaycastFromMouse(out RaycastHit hit))
            return hit.point;
        return Vector3.zero;
    }
}


