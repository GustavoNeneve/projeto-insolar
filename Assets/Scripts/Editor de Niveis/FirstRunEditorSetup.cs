#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class FirstRunEditorSetup : MonoBehaviour
{
    [MenuItem("Ferramentas/Setup Inicial do Editor de Fases")]
    public static void CreateEditorUI()
    {
        // Cria Canvas
        GameObject canvasGO = new GameObject("EditorCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.layer = LayerMask.NameToLayer("UI");

        // Cria EventSystem
        if (FindFirstObjectByType<EventSystem>() == null)
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem));
            eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
#endif
        }

        // Painel de Ferramentas
        GameObject toolPanel = CreatePanel("ToolPanel", canvasGO.transform, new Vector2(160, 400), new Vector2(80, -200));
        // Botões de ferramentas
        CreateButton("BrushButton", toolPanel.transform, "Pincel", new Vector2(0, 120));
        CreateButton("FillButton", toolPanel.transform, "Preencher", new Vector2(0, 60));
        CreateButton("SelectButton", toolPanel.transform, "Selecionar", new Vector2(0, 0));
        // Snap to Grid
        CreateToggle("SnapToGridToggle", toolPanel.transform, "Snap to Grid", new Vector2(0, -60));
        // Undo/Redo
        CreateButton("UndoButton", toolPanel.transform, "Desfazer", new Vector2(0, -120));
        CreateButton("RedoButton", toolPanel.transform, "Refazer", new Vector2(0, -180));

        // Grid de Prefabs
        GameObject prefabPanel = CreatePanel("PrefabPanel", canvasGO.transform, new Vector2(600, 160), new Vector2(300, -80));
        GameObject grid = new GameObject("PrefabGrid", typeof(RectTransform), typeof(GridLayoutGroup));
        grid.transform.SetParent(prefabPanel.transform, false);
        RectTransform gridRect = grid.GetComponent<RectTransform>();
        gridRect.anchorMin = new Vector2(0, 0);
        gridRect.anchorMax = new Vector2(1, 1);
        gridRect.offsetMin = Vector2.zero;
        gridRect.offsetMax = Vector2.zero;
        GridLayoutGroup gridLayout = grid.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(80, 80);
        gridLayout.spacing = new Vector2(8, 8);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1;

        // Feedback visual
        GameObject feedback = CreateText("FeedbackText", canvasGO.transform, "Feedback", new Vector2(0, 200));

        // Adiciona script UITools ao Canvas
        UITools uiTools = canvasGO.AddComponent<UITools>();
        // Aqui você pode fazer o link dos botões criados ao script UITools

        // Botão de salvar interface
        GameObject saveBtn = CreateButton("SaveInterfaceButton", canvasGO.transform, "Salvar Interface", new Vector2(-200, 200));
        saveBtn.GetComponent<Button>().onClick.AddListener(() => SaveEditorUIPrefab(canvasGO));

        Selection.activeGameObject = canvasGO;
        Debug.Log("Interface do Editor criada! Organize os scripts e personalize conforme necessário.");
    }

    static GameObject CreatePanel(string name, Transform parent, Vector2 size, Vector2 anchoredPos)
    {
        GameObject panel = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(parent, false);
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = anchoredPos;
        Image img = panel.GetComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.2f, 0.7f);
        return panel;
    }

    static GameObject CreateButton(string name, Transform parent, string text, Vector2 anchoredPos)
    {
        GameObject btnGO = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btnGO.transform.SetParent(parent, false);
        RectTransform rect = btnGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(120, 40);
        rect.anchoredPosition = anchoredPos;
        Image img = btnGO.GetComponent<Image>();
        img.color = new Color(0.8f, 0.8f, 0.8f, 1f);
#if UNITY_EDITOR
        // Ajusta nome do ícone para botões específicos
        string iconName;
        switch (name)
        {
            case "BrushButton": iconName = "infoIcon"; break;
            case "FillButton": iconName = "BucketFillIcon"; break;
            case "SelectButton": iconName = "CursorIcon"; break;
            case "UndoButton": iconName = "undoIcon"; break;
            case "RedoButton": iconName = "redoIcon"; break;
            default: iconName = name.Replace("Button", "Icon"); break;
        }
        string assetPath = $"Assets/EditorPrefabs/Thumbnails/{iconName}.png";
        Sprite iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        if (iconSprite != null)
        {
            img.sprite = iconSprite;
            img.color = Color.white;
            img.preserveAspect = true;
        }
#endif
        GameObject txtGO = CreateText("Text", btnGO.transform, text, Vector2.zero);
        txtGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        return btnGO;
    }

    static GameObject CreateToggle(string name, Transform parent, string label, Vector2 anchoredPos)
    {
        GameObject toggleGO = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Toggle));
        toggleGO.transform.SetParent(parent, false);
        RectTransform rect = toggleGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(140, 30);
        rect.anchoredPosition = anchoredPos;
        // Configura background
        Image bg = toggleGO.GetComponent<Image>();
        bg.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        Toggle toggle = toggleGO.GetComponent<Toggle>();
        toggle.targetGraphic = bg;
        // Cria checkmark
        GameObject cm = new GameObject("Checkmark", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        cm.transform.SetParent(toggleGO.transform, false);
        RectTransform cmRect = cm.GetComponent<RectTransform>();
        cmRect.anchorMin = new Vector2(0, 0.5f);
        cmRect.anchorMax = new Vector2(0, 0.5f);
        cmRect.sizeDelta = new Vector2(20, 20);
        cmRect.anchoredPosition = new Vector2(-50, 0);
        Image cmImg = cm.GetComponent<Image>();
        cmImg.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Checkmark.psd");
        cmImg.color = Color.white;
        toggle.graphic = cmImg;
        // Label
        GameObject labelGO = CreateText("Label", toggleGO.transform, label, new Vector2(10, 0));
        RectTransform lblRect = labelGO.GetComponent<RectTransform>();
        lblRect.anchoredPosition = new Vector2(10, 0);
        TextMeshProUGUI lbl = labelGO.GetComponent<TextMeshProUGUI>();
        lbl.alignment = TextAlignmentOptions.MidlineLeft;
        return toggleGO;
    }

    static GameObject CreateText(string name, Transform parent, string text, Vector2 anchoredPos)
    {
        GameObject txtGO = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        txtGO.transform.SetParent(parent, false);
        RectTransform rect = txtGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(160, 40);
        rect.anchoredPosition = anchoredPos;
        TextMeshProUGUI txt = txtGO.GetComponent<TextMeshProUGUI>();
        txt.text = text;
        txt.fontSize = 18;
        txt.alignment = TextAlignmentOptions.Center;
        txt.color = Color.white;
        return txtGO;
    }

    public static void SaveEditorUIPrefab(GameObject canvasGO)
    {
        string path = "Assets/EditorUIPrefab.prefab";
        PrefabUtility.SaveAsPrefabAssetAndConnect(canvasGO, path, InteractionMode.UserAction);
        Debug.Log("Interface salva como prefab em: " + path);
    }
}
#endif
