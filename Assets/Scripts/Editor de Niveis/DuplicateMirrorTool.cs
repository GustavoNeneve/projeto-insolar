using UnityEngine;

public class DuplicateMirrorTool : BaseTool
{
    public override void OnToolActivated() { }
    public override void OnToolDeactivated() { }

    public override void OnMouseClick()
    {
        if (RaycastFromMouse(out RaycastHit hit))
        {
            GameObject target = hit.collider.gameObject;
            // Duplicar
            GameObject clone = Instantiate(target, target.transform.position, target.transform.rotation);
            // Espelhar no eixo X
            Vector3 scale = clone.transform.localScale;
            scale.x *= -1;
            clone.transform.localScale = scale;
            // Registrar no Undo/Redo
        }
    }

    public override void OnMouseDrag() { }
    public override void OnMouseUp() { }
}
