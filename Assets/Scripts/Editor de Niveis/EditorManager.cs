using UnityEngine;
using System.Collections.Generic;

public class EditorManager : MonoBehaviour
{
    public static EditorManager Instance { get; private set; }

    public enum ToolType { None, Brush, Fill, Select, Move, Eyedropper, Eraser, ScatterBrush, DuplicateMirror, RaiseLower }
    public ToolType ActiveTool { get; private set; } = ToolType.None;
    public GameObject SelectedPrefab { get; private set; }

    // Snap to Grid settings
    public bool SnapEnabled { get; private set; } = true;
    public float GridSize { get; private set; } = 1f;

    // Objetos atualmente selecionados (usado para agrupar e hierarquia)
    public List<GameObject> SelectedObjects { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveTool(ToolType tool)
    {
        ActiveTool = tool;
        // Notificar ferramentas e UI
    }

    public void SetSelectedPrefab(GameObject prefab)
    {
        SelectedPrefab = prefab;
        // Notificar UI
    }
    // Add this method to the EditorManager class
    public Vector3 SnapToGrid(Vector3 position)
    {
        // Exemplo: Snap para a grade inteira. Ajuste para usar tamanho de célula se necessário.
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
    }

    // Tool activation methods
    public void ActivateBrushTool() => SetActiveTool(ToolType.Brush);
    public void ActivateFillTool() => SetActiveTool(ToolType.Fill);
    public void ActivateSelectTool() => SetActiveTool(ToolType.Select);
    public void ActivateMoveTool() => SetActiveTool(ToolType.None); // Move handled by SelectTool as gizmo
    public void ActivateEyedropperTool() => SetActiveTool(ToolType.None); // Implement EyedropperTool
    public void ActivateEraserTool() => SetActiveTool(ToolType.None); // Implement EraserTool
    public void ActivateScatterTool() => SetActiveTool(ToolType.Brush); // Scatter handled in BrushTool

    public void ToggleSnapGrid(bool enabled) { SnapEnabled = enabled; }
    public void SetGridSize(float size) { GridSize = size; }

    // Undo/Redo
    public void UndoAction() => UnityEngine.Object.FindFirstObjectByType<UndoRedoManager>()?.Undo();
    public void RedoAction() => UnityEngine.Object.FindFirstObjectByType<UndoRedoManager>()?.Redo();

    // Runtime Save with specified name
    public void SaveLayoutRuntime(string layoutName) => UnityEngine.Object.FindFirstObjectByType<RuntimeSaver>()?.SaveSceneLayout(layoutName);

    // Runtime Save with automatic timestamp name
    public void SaveLayoutRuntime() => SaveLayoutRuntime("Layout_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"));

    // Atualiza seleção (chamado pelo SelectTool)
    public void UpdateSelection(List<GameObject> objects)
    {
        SelectedObjects = objects;
        // Notificar UI de hierarquia, etc.
    }

    /// <summary>
    /// Agrupa os objetos selecionados sob um novo GameObject.
    /// </summary>
    public void GroupSelected()
    {
        if (SelectedObjects.Count < 1) return;
        GameObject groupGO = new GameObject("Group_");
        groupGO.transform.parent = null; // raiz
        foreach (var obj in SelectedObjects)
        {
            obj.transform.parent = groupGO.transform;
        }
        // Seleciona o grupo
        SetActiveTool(ToolType.None);
        SelectedObjects = new List<GameObject> { groupGO };
        // Notificar UI
    }

    /// <summary>
    /// Desagrupa o objeto selecionado se for um grupo.
    /// </summary>
    public void UngroupSelected()
    {
        foreach (var obj in SelectedObjects)
        {
            if (obj.transform.childCount > 0)
            {
                var children = new List<Transform>();
                foreach (Transform c in obj.transform) children.Add(c);
                foreach (var c in children)
                {
                    c.parent = null;
                }
                GameObject.Destroy(obj);
            }
        }
        SelectedObjects.Clear();
        // Notificar UI
    }

    // Métodos para eventos importantes, integração com UI, etc.
}
