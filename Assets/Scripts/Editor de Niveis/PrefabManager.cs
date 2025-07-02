using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PrefabCategory
{
    public string categoryName;
    public List<GameObject> prefabs = new List<GameObject>();
}

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance { get; private set; }
    public List<PrefabCategory> categories = new List<PrefabCategory>();
    public Dictionary<GameObject, Sprite> prefabThumbnails = new Dictionary<GameObject, Sprite>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        // Exemplo: Carregar prefabs de Resources/EditorPrefabs/Categorias
        string[] categoryFolders = { "Cenarios", "Props", "Portais", "SpawnAreas" };
        foreach (var folder in categoryFolders)
        {
            GameObject[] prefabs = Resources.LoadAll<GameObject>($"EditorPrefabs/{folder}");
            var cat = new PrefabCategory { categoryName = folder };
            cat.prefabs.AddRange(prefabs);
            categories.Add(cat);
            // Tenta carregar thumbnails (deve ter imagens com mesmo nome do prefab em Resources/EditorPrefabs/Thumbnails)
            foreach (var prefab in prefabs)
            {
                Sprite thumb = Resources.Load<Sprite>($"EditorPrefabs/Thumbnails/{prefab.name}");
                if (thumb != null)
                    prefabThumbnails[prefab] = thumb;
            }
        }
    }

    public List<PrefabCategory> GetCategories()
    {
        return categories;
    }

    public Sprite GetThumbnail(GameObject prefab)
    {
        if (prefabThumbnails.TryGetValue(prefab, out var sprite))
            return sprite;
        return null;
    }

    /// <summary>
    /// Retorna um prefab pelo nome (procura em todas as categorias).
    /// </summary>
    public GameObject GetPrefabByName(string name)
    {
        foreach (var cat in categories)
        {
            foreach (var prefab in cat.prefabs)
            {
                if (prefab.name == name)
                    return prefab;
            }
        }
        return null;
    }
}
