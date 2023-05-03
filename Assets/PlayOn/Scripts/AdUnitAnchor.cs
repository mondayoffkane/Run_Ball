using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class AdUnitAnchor : UIBehaviour
{
    private RectTransform _rect;
    private Canvas _canvas;
    private RectTransform _canvasRect;
#if UNITY_EDITOR
    private Dictionary<PlayOnSDK.Position, Vector3> _positions = new Dictionary<PlayOnSDK.Position, Vector3>();
    private float _maxPXSize;
    private float _minPXSize;
    
    private Vector3 _savedPosition;
    private Vector2 _savedAnchorMin;
    private Vector2 _savedAnchorMax;
    private Vector2 _savedSizeDelta;
    private Vector2 _savedCanvasDelta;

    private bool _isInitialized = false;
    private Coroutine _initCoroutine;
    
    private bool _isUnitFixed = false;
    private bool _isHierarchyDirty = true;
    private bool _isDimensionsDirty = true;

    private bool _isErrorMessageShown = false;
#endif

    protected override void Awake()
    {
        if (Application.isPlaying)
        {
            GetComponent<Image>().enabled = false;
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
    }

#if UNITY_EDITOR
    protected override void OnEnable()
    {
        _isInitialized = false;

        if(_initCoroutine == null)
            _initCoroutine = StartCoroutine(Initialize());
        
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    protected override void OnDisable()
    {
        EditorApplication.hierarchyChanged -= OnHierarchyChanged;
    }

    private void OnHierarchyChanged()
    {
        if(!IsActive() || !_isInitialized || _isHierarchyDirty)
            return;
        
        _isHierarchyDirty = true;
    }

    protected override void OnBeforeTransformParentChanged()
    {
        if(!IsActive() || !_isInitialized || _isHierarchyDirty)
            return;
        
        if(IsProperHierarchy())
            _savedPosition = _rect.position;
    }
    
    protected override void OnRectTransformDimensionsChange()
    {
        if(!IsActive() || !_isInitialized || _isDimensionsDirty)
            return;

        _isDimensionsDirty = true;
    }
    
    private void Update()
    {
        if (!_isInitialized)
        {
            if(_initCoroutine == null)
                _initCoroutine = StartCoroutine(Initialize());
            return;
        }

        _isUnitFixed = false;

        CheckDimensions();
        CheckHierarchy();

        ResetRotationAndScale();
        StayInLines();

        if (!_isUnitFixed && IsProperHierarchy())
            _savedPosition = _rect.position;
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
        _initCoroutine = null;

        _rect = GetComponent<RectTransform>();
        if (!_rect || !_rect.parent)
        {
            if (!_isErrorMessageShown)
            {
                _isErrorMessageShown = true;
                ShowErrorInConsole("Parent is NULL");
            }
            yield break;
        }

        _canvas = _rect.parent.GetComponent<Canvas>();
        if (!_canvas)
        {
            if (!_isErrorMessageShown)
            {
                _isErrorMessageShown = true;
                ShowErrorInConsole("Wrong Parent");
            }
            yield break;
        }

        _canvasRect = _canvas.GetComponent<RectTransform>();
        if(!_canvasRect)
            yield break;

        _savedCanvasDelta = _canvasRect.sizeDelta;
        _savedPosition = _rect.position;
        _savedAnchorMin = _rect.anchorMin;
        _savedAnchorMax = _rect.anchorMax;
        
        _isInitialized = true;
        _isErrorMessageShown = false;

        _isHierarchyDirty = true;
        CheckHierarchy();
        
        CalculateScreenSize();
        SetUnitSize();

        _savedPosition = _rect.position;
    }

    private void CheckDimensions()
    {
        if(!_isDimensionsDirty)
            return;

        if (_savedCanvasDelta != _canvasRect.sizeDelta)
        {
            _isDimensionsDirty = false;
            _isInitialized = false;
            
            if (_initCoroutine == null)
                _initCoroutine = StartCoroutine(Initialize());
            
            return;
        }

        if (IsAnchorsChanged())
        {
            FixAnchors();
            _isUnitFixed = true;
        }

        if (!_savedSizeDelta.Equals(_rect.sizeDelta))
        {
            SetUnitSize();
            
            _rect.position = _savedPosition;
            _isUnitFixed = true;
        }

        if (!_rect.pivot.Equals(new Vector2(0.5f, 0.5f)))
        {
            _rect.pivot = new Vector2(0.5f, 0.5f);
            _isUnitFixed = true;
        }
        
        _isDimensionsDirty = false;
    }
    
    private void CheckHierarchy()
    {
        if(!_isHierarchyDirty)
            return;
        
        if (!_canvas)
        {
            if (!rectTransform.parent)
            {
                ShowErrorInConsole("Parent is NULL");
                return;
            }
            
            _canvas = rectTransform.parent.GetComponent<Canvas>();
            if (!_canvas)
            {
                ShowErrorInConsole("Wrong Parent");
                return;
            }
        }

        if(!_canvasRect)
            _canvasRect = _canvas.GetComponent<RectTransform>();
        
        FixHierarchy();
        _isHierarchyDirty = false;
    }
    
    private void FixHierarchy()
    {
        if (rectTransform.parent != _canvas.transform)
        {
            rectTransform.SetParent(_canvas.transform);
            _rect.position = _savedPosition;
            _isUnitFixed = true;
        }

        if (_canvasRect.childCount - 1 != rectTransform.GetSiblingIndex())
        {
            rectTransform.SetAsLastSibling();
            _rect.position = _savedPosition;
            _isUnitFixed = true;
        }
    }

    private bool IsProperHierarchy()
    {
        if (!_rect || !_rect.parent)
            return false;
        
        if (!_canvas)
            return false;

        if (!_canvasRect)
            return false;

        if (_rect.parent != _canvasRect)
            return false;

        return true;
    }

    private void GetCorners()
    {
        if(!_canvas || !_canvasRect)
            return;
        
        Vector3[] v = new Vector3[4];
        _canvasRect.GetLocalCorners(v);
        _positions = new Dictionary<PlayOnSDK.Position, Vector3>();
        _positions.Add(PlayOnSDK.Position.CenterLeft, new Vector3(v[0].x + _rect.sizeDelta.x / 2, 0f, 0f));
        _positions.Add(PlayOnSDK.Position.CenterRight, new Vector3(v[2].x - _rect.sizeDelta.x / 2, 0f, 0f));
        _positions.Add(PlayOnSDK.Position.BottomCenter, new Vector3(0f, v[0].y + _rect.sizeDelta.x / 2, 0f));
        _positions.Add(PlayOnSDK.Position.BottomLeft,
            new Vector3(v[0].x + _rect.sizeDelta.x / 2, v[0].y + _rect.sizeDelta.x / 2, 0f));
        _positions.Add(PlayOnSDK.Position.BottomRight,
            new Vector3(v[2].x - _rect.sizeDelta.x / 2, v[0].y + _rect.sizeDelta.x / 2, 0f));
        _positions.Add(PlayOnSDK.Position.TopCenter, new Vector3(0f, v[1].y - _rect.sizeDelta.x / 2, 0f));
        _positions.Add(PlayOnSDK.Position.TopLeft,
            new Vector3(v[0].x + _rect.sizeDelta.x / 2, v[1].y - _rect.sizeDelta.x / 2, 0f));
        _positions.Add(PlayOnSDK.Position.TopRight,
            new Vector3(v[2].x - _rect.sizeDelta.x / 2, v[1].y - _rect.sizeDelta.x / 2, 0f));
        _positions.Add(PlayOnSDK.Position.Centered, Vector3.zero);
    }
    
    private void ShowErrorInConsole(string message) {
        var errorMessage = message + ". Put PlayOnAdAnchor in Canvas";
        PlayOnSDK.LogE(PlayOnSDK.LogLevel.Debug, errorMessage);
    }

    private PlayOnSDK.Position GetClosest(Vector3 startPosition, Dictionary<PlayOnSDK.Position, Vector3> pickups)
    {
        PlayOnSDK.Position location = PlayOnSDK.Position.Centered;
        float closestDistanceSqr = Mathf.Infinity;
        
        foreach (var potentialTarget in pickups)
        {
            Vector3 directionToTarget = potentialTarget.Value - startPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                location = potentialTarget.Key;
            }
        }

        return location;
    }

    private void StayInLines()
    {
        if (_canvas == null)
            return;

        GetCorners();
        
        var location = GetClosest(_rect.localPosition, _positions);
        var loc = _positions[location];
        var xoffset = _rect.localPosition.x - loc.x;
        var yoffset = _rect.localPosition.y - loc.y;
        
        switch (location)
        {
            case PlayOnSDK.Position.Centered:
                break;
            case PlayOnSDK.Position.CenterRight:
                if (xoffset > 0)
                    _rect.localPosition = new Vector3(_positions[PlayOnSDK.Position.CenterRight].x,
                        _rect.localPosition.y, 0);
                break;
            case PlayOnSDK.Position.CenterLeft:
                if (xoffset < 0)
                    _rect.localPosition = new Vector3(_positions[PlayOnSDK.Position.CenterLeft].x,
                        _rect.localPosition.y, 0);
                break;
            case PlayOnSDK.Position.TopCenter:
                if (yoffset > 0)
                    _rect.localPosition = new Vector3(_rect.localPosition.x,
                        _positions[PlayOnSDK.Position.TopCenter].y, 0);
                break;
            case PlayOnSDK.Position.TopRight:
                if (yoffset > 0)
                    _rect.localPosition = new Vector3(_rect.localPosition.x,
                        _positions[PlayOnSDK.Position.TopCenter].y, 0);
                if (xoffset > 0)
                    _rect.localPosition = new Vector3(_positions[PlayOnSDK.Position.TopRight].x,
                        _rect.localPosition.y, 0);
                break;
            case PlayOnSDK.Position.TopLeft:
                if (yoffset > 0)
                    _rect.localPosition = new Vector3(_rect.localPosition.x,
                        _positions[PlayOnSDK.Position.TopCenter].y, 0);
                if (xoffset < 0)
                    _rect.localPosition = new Vector3(_positions[PlayOnSDK.Position.TopLeft].x,
                        _rect.localPosition.y, 0);
                break;
            case PlayOnSDK.Position.BottomCenter:
                if (yoffset < 0)
                    _rect.localPosition = new Vector3(_rect.localPosition.x,
                        _positions[PlayOnSDK.Position.BottomCenter].y, 0);
                break;
            case PlayOnSDK.Position.BottomRight:
                if (yoffset < 0)
                    _rect.localPosition = new Vector3(_rect.localPosition.x,
                        _positions[PlayOnSDK.Position.BottomRight].y, 0);
                if (xoffset > 0)
                    _rect.localPosition = new Vector3(_positions[PlayOnSDK.Position.BottomRight].x,
                        _rect.localPosition.y, 0);
                break;
            case PlayOnSDK.Position.BottomLeft:
                if (yoffset < 0)
                    _rect.localPosition = new Vector3(_rect.localPosition.x,
                        _positions[PlayOnSDK.Position.BottomLeft].y, 0);
                if (xoffset < 0)
                    _rect.localPosition = new Vector3(_positions[PlayOnSDK.Position.BottomLeft].x,
                        _rect.localPosition.y, 0);
                break;
        }
    }

    private void CalculateScreenSize()
    {
        if(!_canvas)
            return;

        PlayOnSDK.SetOptimalDPI();

        float minScreenSize = 70f * (PlayOnSDK.GetUnityEditorDPI() / 160f);
        float maxScreenSize = 120f * (PlayOnSDK.GetUnityEditorDPI() / 160f);

        Vector2 sizeDelta = _canvasRect.sizeDelta;
                
        _minPXSize = minScreenSize / Screen.width * sizeDelta.x;
        _maxPXSize = maxScreenSize / Screen.width * sizeDelta.x;
    }

    private void SetUnitSize()
    {
        float unitSize = Mathf.Clamp(_rect.sizeDelta.x, _minPXSize, _maxPXSize);

        _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, unitSize);
        _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, unitSize);
        _rect.ForceUpdateRectTransforms();

        _savedSizeDelta = _rect.sizeDelta;
    }

    private bool IsAnchorsChanged()
    {
        if (_savedAnchorMax != _rect.anchorMax)
            return true;

        if (_savedAnchorMin != _rect.anchorMin)
            return true;

        return false;
    }

    private void FixAnchors()
    {
        bool isNeedToRestorePosition = false;
        if (_rect.anchorMax != _rect.anchorMin)
        {
            _savedPosition = _rect.position;
            isNeedToRestorePosition = true;
        }

        if (_savedAnchorMax != _rect.anchorMax) 
            _rect.anchorMin = _rect.anchorMax;
        else if (_savedAnchorMin != _rect.anchorMin) 
            _rect.anchorMax = _rect.anchorMin;

        _savedAnchorMin = _rect.anchorMin;
        _savedAnchorMax = _rect.anchorMax;

        _rect.sizeDelta = _savedSizeDelta;

        if (isNeedToRestorePosition)
            _rect.position = _savedPosition;
    }

    private void ResetRotationAndScale()
    {
        _rect.rotation = Quaternion.identity;
        _rect.localScale = Vector3.one;
    }

#endif

    public RectTransform rectTransform
    {
        get
        {
            if (!_rect) _rect = (RectTransform)transform;
            return _rect;
        }
    }

    public Canvas canvas
    {
        get
        {
            if (!_canvas) _canvas = transform.parent.GetComponent<Canvas>();
            return _canvas;
        }
    }
}