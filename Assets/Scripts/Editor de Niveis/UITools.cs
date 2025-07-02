using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UITools : MonoBehaviour
{
    public static UITools Instance { get; private set; }

    [Header("Tool Buttons")]
    public Button brushButton;
    public Button fillButton;
    public Button selectButton;
    [Header("Prefab Grid")]
    public Transform prefabGridParent;
    public GameObject prefabButtonPrefab;
    public TMP_Dropdown categoryDropdown;
    [Header("Snap to Grid")]
    public Toggle snapToGridToggle;
    public Slider gridSizeSlider;
    [Header("Undo/Redo")]
    public Button undoButton;
    public Button redoButton;
    [Header("Feedback")]
    public TMP_Text feedbackText;

    private List<GameObject> currentPrefabs = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    void Start()
    {
        // Preencher categorias no dropdown
        var categories = PrefabManager.Instance.GetCategories();
        categoryDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var cat in categories)
            options.Add(cat.categoryName);
        categoryDropdown.AddOptions(options);
        categoryDropdown.onValueChanged.AddListener(OnCategoryChanged);
        OnCategoryChanged(0);
    }

    void OnCategoryChanged(int index)
    {
        foreach (Transform child in prefabGridParent)
            Destroy(child.gameObject);
        var cat = PrefabManager.Instance.GetCategories()[index];
        currentPrefabs = cat.prefabs;
        foreach (var prefab in currentPrefabs)
        {
            var btn = Instantiate(prefabButtonPrefab, prefabGridParent);
            var img = btn.GetComponentInChildren<Image>();
            var txt = btn.GetComponentInChildren<TMP_Text>();
            img.sprite = PrefabManager.Instance.GetThumbnail(prefab);
            txt.text = prefab.name;
            btn.GetComponent<Button>().onClick.AddListener(() => EditorManager.Instance.SetSelectedPrefab(prefab));
        }
    }

    public void ShowFeedback(string message)
    {
        feedbackText.text = message;
    }

    // Métodos para atualizar UI, preencher grid de prefabs, etc.
}
