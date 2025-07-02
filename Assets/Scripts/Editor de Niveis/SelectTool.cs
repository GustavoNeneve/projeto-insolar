using UnityEngine;
using System.Collections.Generic;

public class SelectTool : BaseTool
{
    private List<GameObject> selectedObjects = new List<GameObject>();

    public override void OnToolActivated() { }
    public override void OnToolDeactivated() { }

    public override void OnMouseClick()
    {
        // Selecionar objeto sob o mouse, suporte a múltipla seleção
    }

    public override void OnMouseDrag()
    {
        // Mover objetos selecionados
    }

    public override void OnMouseUp() { }

    private void ShowGizmos()
    {
        // Exibir gizmos de manipulação
    }

    private void DeleteSelected()
    {
        // Deletar objetos selecionados, registrar Undo/Redo
    }
}
