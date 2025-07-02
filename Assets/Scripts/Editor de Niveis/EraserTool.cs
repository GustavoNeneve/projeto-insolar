using UnityEngine;

public class EraserTool : BaseTool
{
    public override void OnToolActivated() { }
    public override void OnToolDeactivated() { }

    public override void OnMouseClick()
    {
        if (RaycastFromMouseUI(out RaycastHit hit))
        {
            Destroy(hit.collider.gameObject);
            // Registrar no Undo/Redo se implementado
        }
    }

    public override void OnMouseDrag()
    {
        if (RaycastFromMouseUI(out RaycastHit hit))
        {
            Destroy(hit.collider.gameObject);
        }
    }

    public override void OnMouseUp() { }
}
