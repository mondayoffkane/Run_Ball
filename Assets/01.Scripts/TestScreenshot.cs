using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Xml.Linq;
using DG.Tweening.Core.Easing;

public class TestScreenshot : MonoBehaviour
{
    public string folderName = "ScreenShots";
    public string fileName = "MyScreenShot";
    public string extName = "png";

    private string RootPath
    {
        get
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return Application.dataPath;
#elif UNITY_ANDROID
                return $"/storage/emulated/0/DCIM/{Application.productName}/";
                //return Application.persistentDataPath;
#endif
        }
    }

    public string FolderPath => $"{RootPath}/{folderName}";
    public string TotalPath => $"{FolderPath}/{fileName}_{DateTime.Now.ToString("MMdd_HHmmss")}.{extName}";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(TakeShot());
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Screen.SetResolution(500, 500, false,60);
            //Screen.fullScreen = true;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Screen.SetResolution(1000, 1000, false,60);
            //Screen.fullScreen = false;
            
        }
    }

    IEnumerator TakeShot()
    {
        yield return new WaitForEndOfFrame();
        string totalPath = TotalPath; // 프로퍼티 참조 시 시간에 따라 이름이 결정되므로 캐싱

        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);

        // 현재 스크린으로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        screenTex.ReadPixels(area, 0, 0);



        bool succeeded = true;
        try
        {
            // 폴더가 존재하지 않으면 새로 생성
            if (Directory.Exists(FolderPath) == false)
            {
                Directory.CreateDirectory(FolderPath);
            }

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
        {
            Debug.Log($"Screen Shot Saved : {totalPath}");

        }

    }
}
