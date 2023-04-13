#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;

// Need Odin Package

public class ScreenShotMaker : MonoBehaviour
{
    public string folderName = "ScreenShots";
    [ReadOnly] public string folderNameLast = "1242*2688";
    string fileName = "0";
    public string extName = "png";
    string totalPath;

    int _count = 0;
    private string RootPath
    {
        get
        {
            return Application.dataPath;
        }
    }

    public string FolderPath => $"{RootPath}/{folderName}/{folderNameLast}";
    public string TotalPath => $"{FolderPath}//{fileName}.{extName}";

    // ////////////////////////////////////////////////////////////////////////////////////////////
    // ////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenShot();
        }
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////
    // ////////////////////////////////////////////////////////////////////////////////////////////

    [Button(90), GUIColor(0f, 1f, 0f)]
    void ScreenShot()
    {
        StartCoroutine(TakeShot());
    }

    IEnumerator TakeShot()
    {
        EditorApplication.isPaused = false;
        Time.timeScale = 0f;

        yield return new WaitForEndOfFrame();

        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);
        folderNameLast = $"{screenTex.width}*{screenTex.height}";
        _count = 0;

        if (Directory.Exists(FolderPath))
        {
            foreach (string _file in Directory.GetFiles(FolderPath))
            {
                if (Path.GetExtension(_file) == $".{extName}")
                {
                    _count++;
                }
            }
        }

        fileName = _count.ToString();
        totalPath = TotalPath;
        screenTex.ReadPixels(area, 0, 0);// 현재 스크린으로부터 지정 영역의 픽셀들을 텍스쳐에 저장


        bool succeeded = true;
        try
        {
            // 폴더가 존재하지 않으면 새로 생성
            if (Directory.Exists(FolderPath) == false)
            { Directory.CreateDirectory(FolderPath); }

            // 스크린샷 저장
            File.WriteAllBytes(totalPath, screenTex.EncodeToPNG());
        }
        catch (Exception e)
        {
            succeeded = false;
            Debug.LogWarning($"Screen Shot Save Failed : {totalPath}");
            Debug.LogWarning(e);
        }

        // 마무리 작업
        Destroy(screenTex);

        if (succeeded)
        { Debug.Log($"Screen Shot Saved : {totalPath}"); }

        _count++;
        EditorApplication.isPaused = true;
        Time.timeScale = 1f;
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Gigantic), GUIColor(0.4f, 0.8f, 1f)]
    void OpenFolder()
    {
        if (Directory.Exists($"{RootPath}/{folderName}"))
        {
            EditorUtility.OpenWithDefaultApp($"{RootPath}/{folderName}");
        }
        else
        {
            Directory.CreateDirectory($"{RootPath}/{folderName}");
        }
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Gigantic), GUIColor(1f, 0.2f, 0f)]
    void Delete_Folder()
    {
        if (Directory.Exists(FolderPath))
        {
            DeleteDirectory($"{RootPath}/{folderName}");
        }
    }


    public static void DeleteDirectory(string path)
    {
        foreach (string directory in Directory.GetDirectories(path))
        {
            DeleteDirectory(directory);
        }

        try
        { Directory.Delete(path, true); }
        catch (IOException)
        { Directory.Delete(path, true); }
        catch (UnauthorizedAccessException)
        { Directory.Delete(path, true); }
    }

}
#endif