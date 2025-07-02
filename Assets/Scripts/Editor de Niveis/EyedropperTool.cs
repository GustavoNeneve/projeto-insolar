using UnityEngine;

public class EyedropperTool : BaseTool
{
    public override void OnToolActivated() { }
    public override void OnToolDeactivated() { }

    public override void OnMouseClick()
    {
        if (RaycastFromMouseUI(out RaycastHit hit))
        {
            var picked = hit.collider.gameObject;
            // Define o prefab ativo baseado no objeto clicado
            EditorManager.Instance.SetSelectedPrefab(picked);
            UITools.Instance?.ShowFeedback($"Prefab selecionado: {picked.name}");
        }
    }

    public override void OnMouseDrag() { }
    public override void OnMouseUp() { }
}
