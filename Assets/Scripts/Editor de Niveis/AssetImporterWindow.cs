#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetImporterWindow : EditorWindow
{
    [MenuItem("Ferramentas/Importador de Modelos (.obj)")]
    public static void ShowWindow()
    {
        GetWindow<AssetImporterWindow>("Importador de OBJ");
    }

    private void OnGUI()
    {
        GUILayout.Label("Arraste arquivos OBJ aqui", EditorStyles.boldLabel);
        var dropArea = GUILayoutUtility.GetRect(0f, 100f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop Zone: .obj files");

        var e = Event.current;
        if ((e.type == EventType.DragUpdated || e.type == EventType.DragPerform) && dropArea.Contains(e.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (e.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var path in DragAndDrop.paths)
                {
                    if (Path.GetExtension(path).ToLower() == ".obj")
                    {
                        ImportObjAsPrefab(path);
                    }
                }
            }
            e.Use();
        }
    }

    private static void ImportObjAsPrefab(string sourcePath)
    {
        string fileName = Path.GetFileName(sourcePath);
        string modelsFolder = "Assets/ImportedModels";
        if (!AssetDatabase.IsValidFolder(modelsFolder))
            AssetDatabase.CreateFolder("Assets", "ImportedModels");

        string destModelPath = Path.Combine(modelsFolder, fileName).Replace("\\", "/");
        FileUtil.CopyFileOrDirectory(sourcePath, destModelPath);
        AssetDatabase.ImportAsset(destModelPath, ImportAssetOptions.Default);

        // Carrega o modelo importado
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(destModelPath);
        if (model != null)
        {
            // Instancia em cena temporariamente para criar o prefab
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(model);

            // Cria pasta de prefabs
            string prefabFolder = modelsFolder + "/Prefabs";
            if (!AssetDatabase.IsValidFolder(prefabFolder))
                AssetDatabase.CreateFolder(modelsFolder, "Prefabs");

            string prefabPath = prefabFolder + "/" + Path.GetFileNameWithoutExtension(fileName) + ".prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(instance, prefabPath, InteractionMode.UserAction);
            GameObject.DestroyImmediate(instance);
        }

        AssetDatabase.Refresh();
        Debug.Log($"Importado e convertido em prefab: {fileName}");
    }
}
#endif
