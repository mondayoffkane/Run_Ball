#if UNITY_EDITOR || UNITY_STANDALONE_OSX
using UnityEngine;
using UnityEditor;
public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexture = null;
    [SerializeField] Texture2D cursorTexture_clicked = null;
    [SerializeField] Vector2 hotSpot;
    bool isShowCursor = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isShowCursor = !isShowCursor;
            Cursor.SetCursor(isShowCursor == true ? cursorTexture : null, hotSpot, CursorMode.Auto);
        }

        if (Input.GetMouseButtonDown(0) && isShowCursor)
        {
            Cursor.SetCursor(cursorTexture_clicked, hotSpot, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0) && isShowCursor)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        }
    }
}

public class CursorEditor
{
    [MenuItem("Jerry/MakeCursor")]
    public static void MakeCursor()
    {
        GameObject go = GameObject.Instantiate(Resources.Load("CursorManager")) as GameObject;
        go.name = "Jerry_Cursor";
    }
}
#endif


