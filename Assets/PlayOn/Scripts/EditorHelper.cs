#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorHelper
{
    public static string GetAssetBasedPath(string localPath)
    {
        string path = "";
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        
        for(int i = 0; i < allAssetPaths.Length; i++)
        {
            if (allAssetPaths[i].EndsWith(localPath))
                path = allAssetPaths[i];
        }
        
        return path;
    }
}
#endif
