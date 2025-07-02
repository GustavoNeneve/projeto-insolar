using UnityEngine;

public class RaiseLowerTool : BaseTool
{
    public float step = 1f;

    public override void OnToolActivated() { }
    public override void OnToolDeactivated() { }

    public override void OnMouseClick()
    {
        if (RaycastFromMouse(out RaycastHit hit))
        {
            var obj = hit.collider.gameObject;
            float direction = Input.GetKey(KeyCode.LeftShift) ? -1f : 1f;
            obj.transform.position += Vector3.up * step * direction;
            // Registrar no Undo/Redo se necessário
        }
    }

    public override void OnMouseDrag() { }
    public override void OnMouseUp() { }
}
