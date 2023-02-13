using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private UI_Scene _main = null;
    public T main<T>() where T : UI_Scene => (T)_main;
    private GameObject _root;
    public GameObject Root
    {
        get
        {
            if (_root == null)
            {
                _root = new GameObject { name = "@UI_Root" };
            }
            return _root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(name);
        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(name);
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _main = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(name);

        go.GetOrAddComponent<GraphicRaycaster>();
        CanvasScaler scaler = go.GetOrAddComponent<CanvasScaler>();
        CanvasScaler sceneScaler = _main.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = sceneScaler.uiScaleMode;
        scaler.referenceResolution = sceneScaler.referenceResolution;
        scaler.screenMatchMode = sceneScaler.screenMatchMode;
        scaler.matchWidthOrHeight = sceneScaler.matchWidthOrHeight;

        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(_root.transform);
        popup.Init();

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        // while (_popupStack.Count > 0)
        //     ClosePopupUI();
        while (_popupStack.Count > 0)
        {
            UI_Popup popup = _popupStack.Pop();
            popup.ClosePopupUI();
            _order--;
        }
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _main = null;
    }
}
