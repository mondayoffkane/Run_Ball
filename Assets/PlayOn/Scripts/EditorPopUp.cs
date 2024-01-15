using UnityEngine;

public class EditorPopUp : MonoBehaviour
{
#if UNITY_EDITOR
    private EditorPopUpType _type;
    public RectTransform rect;
    public Canvas canvas;
    private int _xOffset;
    private int _yOffset;
    private PlayOnSDK.Position _location;
    private Vector2 _size;
    public void ShowPopUp ( EditorPopUpType type, PlayOnSDK.Position location, int xOffset, int yOffset) {
        _type = type;
        _location = location;
        _xOffset = xOffset;
        _yOffset = yOffset;
        if (type == EditorPopUpType.Banner)
            _size = new Vector2(320, 50);
        else
            _size = new Vector2(120, 120);
        SetPosition();
    }
    
    private void SetPosition ()
    {
        float deviceScale = PlayOnSDK.GetDeviceScale();
        
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2((_size.x + 0.5f) * deviceScale / canvas.scaleFactor, (_size.y + 0.5f) * deviceScale / canvas.scaleFactor);
        
       
        var xPos = _xOffset * deviceScale + 0.5f;
        var yPos = _yOffset * deviceScale + 0.5f;
        
        switch (_location)
        {
            case PlayOnSDK.Position.Centered:
                rect.pivot = new Vector2(0.5f, 0.5f);
                yPos = canvas.pixelRect.height / 2 - yPos;
                xPos = canvas.pixelRect.width / 2 - xPos;
                break;
            case PlayOnSDK.Position.BottomLeft:
                rect.pivot = Vector2.zero;
                break;
            case PlayOnSDK.Position.BottomRight:
                rect.pivot = new Vector2(1f, 0f);
                xPos = canvas.pixelRect.width - xPos;
                break;
            case PlayOnSDK.Position.TopLeft:
                rect.pivot = new Vector2(0f, 1f);
                yPos = canvas.pixelRect.height - yPos;
                break;
            case PlayOnSDK.Position.TopRight:
                rect.pivot = new Vector2(1f, 1f);
                yPos = canvas.pixelRect.height - yPos;
                xPos = canvas.pixelRect.width - xPos;
                break;
            case PlayOnSDK.Position.CenterLeft:
                rect.pivot = new Vector2(0f, 0.5f);
                yPos = canvas.pixelRect.height / 2 - yPos;
                break;
            case PlayOnSDK.Position.CenterRight:
                rect.pivot = new Vector2(1f, 0.5f);
                yPos = canvas.pixelRect.height / 2 - yPos;
                xPos = canvas.pixelRect.width - xPos;
                break;
            case PlayOnSDK.Position.BottomCenter:
                rect.pivot = new Vector2(0.5f, 0f);
                xPos = canvas.pixelRect.width / 2 - xPos;
                break;
            case PlayOnSDK.Position.TopCenter:
                rect.pivot = new Vector2(0.5f, 1f);
                xPos = canvas.pixelRect.width / 2 - xPos;
                yPos = canvas.pixelRect.height - yPos;
                break;
        }
        rect.position = new Vector3(xPos, yPos, 0);
    }
#endif
}

public enum EditorPopUpType
{
    Logo,
    Banner
}