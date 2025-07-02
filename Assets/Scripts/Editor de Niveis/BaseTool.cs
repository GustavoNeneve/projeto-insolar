using UnityEngine;

public abstract class BaseTool : MonoBehaviour
{
    protected EditorManager editorManager;
    protected GameObject selectedPrefab;

    protected virtual void Awake()
    {
        editorManager = EditorManager.Instance;
    }

    public abstract void OnToolActivated();
    public abstract void OnToolDeactivated();
    public abstract void OnMouseClick();
    public abstract void OnMouseDrag();
    public abstract void OnMouseUp();

    protected bool RaycastFromMouse(out RaycastHit hit, int layerMask = ~0)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
    }

    protected bool RaycastFromMouseUI(out RaycastHit hit)
    {
        // Raycast ignorando UI, útil para ferramentas como Eyedropper
        if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            hit = default;
            return false;
        }
        return RaycastFromMouse(out hit);
    }
}
