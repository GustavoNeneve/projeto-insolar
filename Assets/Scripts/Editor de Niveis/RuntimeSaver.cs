using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;


public class RuntimeSaver : MonoBehaviour
{
    // Save layout with custom name under Assets/LayoutData
    public void SaveSceneLayout(string layoutName)
    {
        string folder = "Assets/LayoutData";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        var sceneData = CollectSceneData();
        string json = JsonUtility.ToJson(sceneData, true);
        string path = Path.Combine(folder, layoutName + ".json");
        File.WriteAllText(path, json);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    // Load and rebuild layout by name
    public void LoadSceneLayout(string layoutName)
    {
        string path = Path.Combine("Assets/LayoutData", layoutName + ".json");
        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        SceneData data = JsonUtility.FromJson<SceneData>(json);
        // Limpar objetos antigos da cena
        var old = GameObject.FindGameObjectsWithTag("EditorObject");
        foreach (var obj in old) GameObject.Destroy(obj);
        // Instanciar objetos
        foreach (var d in data.objects)
        {
            var prefab = PrefabManager.Instance.GetPrefabByName(d.prefabName);
            if (prefab != null)
            {
                GameObject obj = Instantiate(prefab, d.position, Quaternion.Euler(d.rotation));
                obj.transform.localScale = d.scale;
                obj.tag = "EditorObject";
            }
        }
        // Conexões de portais podem ser recriadas aqui se necessário
    }

    // Coleta dados de cena para serialização
    private SceneData CollectSceneData()
    {
        var objects = GameObject.FindGameObjectsWithTag("EditorObject");
        List<SceneObjectData> data = new List<SceneObjectData>();
        foreach (var obj in objects)
            data.Add(new SceneObjectData { prefabName = obj.name, position = obj.transform.position, rotation = obj.transform.rotation.eulerAngles, scale = obj.transform.localScale });
        // Conexões
        List<SceneConnection> connections = new List<SceneConnection>();
        var portals = UnityEngine.Object.FindFirstObjectByType<PortalConnector>()?.GetComponents<PortalConnector>();
        if (portals != null)
            foreach (var portal in portals)
                connections.Add(new SceneConnection { portalId = portal.portalId, targetSceneId = portal.targetSceneId, targetPortalId = portal.targetPortalId, position = portal.transform.position });
        return new SceneData { objects = data, connections = connections };
    }
}

[System.Serializable]
public class SceneObjectData
{
    public string prefabName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

[System.Serializable]
public class SceneConnection {
    public string portalId;
    public string targetSceneId;
    public string targetPortalId;
    public Vector3 position;
}

[System.Serializable]
public class SceneData
{
    public List<SceneObjectData> objects;
    public List<SceneConnection> connections;
}
