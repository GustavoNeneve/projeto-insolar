using UnityEditor;
using UnityEngine;
using System.IO;

public class PrefabIconImporter : MonoBehaviour
{
    [MenuItem("Ferramentas/Importar Icones do Pokemon DS Map Studio")]
    public static void ImportIcons()
    {
        string sourcePath = "Pokemon-DS-Map-Studio-master/src/main/resources/icons/";
        string destPath = "Assets/EditorPrefabs/Thumbnails/";
        if (!Directory.Exists(destPath))
            Directory.CreateDirectory(destPath);

        string[] iconFiles = Directory.GetFiles(sourcePath, "*.png");
        foreach (var file in iconFiles)
        {
            string fileName = Path.GetFileName(file);
            string destFile = Path.Combine(destPath, fileName);
            File.Copy(file, destFile, true);
        }
        AssetDatabase.Refresh();
        Debug.Log("Ícones importados para " + destPath);
    }
}
