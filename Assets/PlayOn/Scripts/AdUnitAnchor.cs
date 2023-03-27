using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Diagnostics;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class AdUnitAnchor : UIBehaviour
{
    private RectTransform _transform;
    private Canvas _canvas;
#if UNITY_EDITOR
    private Dictionary<PlayOnSDK.Position, Vector3> positions = new Dictionary<PlayOnSDK.Position, Vector3>();
    private float maxPXSize;
    private float minPXSize;


    private Vector3 savedPosition;
    private Vector2 savedWindowSizeDelta = Vector2.zero;
    private Vector2 savedAnchorMin;
    private Vector2 savedAnchorMax;
    private float savedSize;

    private float delayBeforeSave = 1.5f;

    private bool inited = false;
    private bool saved = false;

    protected override void Awake() {
        _transform = (RectTransform) transform;
    }

    protected override void OnEnable() {
        inited = false;
        _transform = (RectTransform) transform;
        OnTransformParentChanged();
        ChangeHierarchy();
        if (_canvas != null)
            PlayOnEditorCoroutinesManager.StartEditorCoroutine(SetOptimalDPI());
        EditorApplication.hierarchyChanged += ChangeHierarchy;
        base.OnEnable();
    }

    protected override void OnDisable() {
        StopAllCoroutines();
        inited = false;
        saved = false;
        EditorApplication.hierarchyChanged -= ChangeHierarchy;
        base.OnDisable();
    }

    protected override void OnValidate() {
        base.OnValidate();
    }

    private void ChangeHierarchy() {
        if (_canvas == null)
        {
            ShowErrorInConsole("Hierarchy changed");
            return;
        }

        if (_canvas.transform.childCount - 1 != _transform.GetSiblingIndex())
        {
            _transform.SetAsLastSibling();
        }
    }

    protected override void OnTransformParentChanged() {
        if (this.transform.parent != null)
        {
            _canvas = this.transform.parent.GetComponent<Canvas>();
            if (_canvas == null)
            {
                ShowErrorInConsole("Parent changed");
            }
        }
        else
        {
            _canvas = null;
            ShowErrorInConsole("Parent changed");
        }

        base.OnTransformParentChanged();
    }

    protected override void OnRectTransformDimensionsChange() {
        if (!this.IsActive() || !inited || !saved)
            return;

        if (isAnchoredChangedFromCode())
        {
            SetSavedSize(savedSize);
            return;
        }

        if (!WindowSizeChanged())
        {
            _transform.position = savedPosition;
        }
        else
        {
            return;
        }
        if ((savedAnchorMax != _transform.anchorMax) || savedAnchorMin != _transform.anchorMin)
        {
            FixAnchors();
        }
  
        FixSize();
        savedSize = GetSizeInDP(_transform.sizeDelta.x);
        base.OnRectTransformDimensionsChange();
    }
    
    private void Update() {
        if (!inited)
            return;
        Freeze();
        if (!WindowSizeChanged())
        {
            if (savedPosition != _transform.position)
            {
                if (saved)
                {
                    savedPosition = _transform.position;
                }

            }
        }
        else
        {
            PlayOnEditorCoroutinesManager.StartEditorCoroutine(SetOptimalDPI());
        }
        StayInLines();
    }


    private void GetCorners() {
        Vector3[] v = new Vector3[4];
        this.transform.parent.GetComponent<RectTransform>().GetLocalCorners(v);
        positions = new Dictionary<PlayOnSDK.Position, Vector3>();
        positions.Add(PlayOnSDK.Position.CenterLeft, new Vector3(v[0].x + _transform.sizeDelta.x / 2, 0f, 0f));
        positions.Add(PlayOnSDK.Position.CenterRight, new Vector3(v[2].x - _transform.sizeDelta.x / 2, 0f, 0f));
        positions.Add(PlayOnSDK.Position.BottomCenter, new Vector3(0f, v[0].y + _transform.sizeDelta.x / 2, 0f));
        positions.Add(PlayOnSDK.Position.BottomLeft,
            new Vector3(v[0].x + _transform.sizeDelta.x / 2, v[0].y + _transform.sizeDelta.x / 2, 0f));
        positions.Add(PlayOnSDK.Position.BottomRight,
            new Vector3(v[2].x - _transform.sizeDelta.x / 2, v[0].y + _transform.sizeDelta.x / 2, 0f));
        positions.Add(PlayOnSDK.Position.TopCenter, new Vector3(0f, v[1].y - _transform.sizeDelta.x / 2, 0f));
        positions.Add(PlayOnSDK.Position.TopLeft,
            new Vector3(v[0].x + _transform.sizeDelta.x / 2, v[1].y - _transform.sizeDelta.x / 2, 0f));
        positions.Add(PlayOnSDK.Position.TopRight,
            new Vector3(v[2].x - _transform.sizeDelta.x / 2, v[1].y - _transform.sizeDelta.x / 2, 0f));
        positions.Add(PlayOnSDK.Position.Centered, Vector3.zero);
    }


    private void ShowErrorInConsole(string message) {
        var errorMessage = message + ". Put PlayonAdAnchor in Canvas";
        PlayOnSDK.LogE(PlayOnSDK.LogLevel.Debug, errorMessage);
    }

    private PlayOnSDK.Position GetClosest(Vector3 startPosition, Dictionary<PlayOnSDK.Position, Vector3> pickups) {
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

    private void StayInLines() {
        if (_canvas == null)
            return;
        _transform.pivot = new Vector2(0.5f, 0.5f);
        GetCorners();
        var location = GetClosest(_transform.localPosition, positions);
        var loc = positions[location];
        var xoffset = _transform.localPosition.x - loc.x;
        var yoffset = _transform.localPosition.y - loc.y;
        switch (location)
        {
            case PlayOnSDK.Position.Centered:
                break;
            case PlayOnSDK.Position.CenterRight:
                if (xoffset > 0)
                {
                    xoffset = 0;
                    this.transform.localPosition = new Vector3(positions[PlayOnSDK.Position.CenterRight].x,
                        this.transform.localPosition.y, 0);
                }

                break;
            case PlayOnSDK.Position.CenterLeft:
                if (xoffset < 0)
                {
                    xoffset = 0;
                    this.transform.localPosition = new Vector3(positions[PlayOnSDK.Position.CenterLeft].x,
                        this.transform.localPosition.y, 0);
                }

                break;
            case PlayOnSDK.Position.TopCenter:
                if (yoffset > 0)
                {
                    yoffset = 0;
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                        positions[PlayOnSDK.Position.TopCenter].y, 0);
                }

                break;
            case PlayOnSDK.Position.TopRight:
                if (yoffset > 0)
                {
                    yoffset = 0;
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                        positions[PlayOnSDK.Position.TopCenter].y, 0);
                }

                if (xoffset > 0)
                {
                    xoffset = 0;
                    this.transform.localPosition = new Vector3(positions[PlayOnSDK.Position.TopRight].x,
                        this.transform.localPosition.y, 0);
                }

                break;
            case PlayOnSDK.Position.TopLeft:
                if (yoffset > 0)
                {
                    yoffset = 0;
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                        positions[PlayOnSDK.Position.TopCenter].y, 0);
                }

                if (xoffset < 0)
                {
                    xoffset = 0;
                    this.transform.localPosition = new Vector3(positions[PlayOnSDK.Position.TopLeft].x,
                        this.transform.localPosition.y, 0);
                }

                break;
            case PlayOnSDK.Position.BottomCenter:
                if (yoffset < 0)
                {
                    yoffset = 0;
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                        positions[PlayOnSDK.Position.BottomCenter].y, 0);
                }

                break;
            case PlayOnSDK.Position.BottomRight:
                if (yoffset < 0)
                {
                    yoffset = 0;
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                        positions[PlayOnSDK.Position.BottomRight].y, 0);
                }

                if (xoffset > 0)
                {
                    xoffset = 0;
                    this.transform.localPosition = new Vector3(positions[PlayOnSDK.Position.BottomRight].x,
                        this.transform.localPosition.y, 0);
                }

                break;
            case PlayOnSDK.Position.BottomLeft:
                if (yoffset < 0)
                {
                    yoffset = 0;
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x,
                        positions[PlayOnSDK.Position.BottomLeft].y, 0);
                }

                if (xoffset < 0)
                {
                    xoffset = 0;
                    this.transform.localPosition = new Vector3(positions[PlayOnSDK.Position.BottomLeft].x,
                        this.transform.localPosition.y, 0);
                }

                break;
        }
    }


    private bool WindowSizeChanged() {
        if (_canvas == null)
            return false;
        if (_canvas.pixelRect.width != savedWindowSizeDelta.x || _canvas.pixelRect.height != savedWindowSizeDelta.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public IEnumerator SetOptimalDPI() {
        inited = false;
        yield return new WaitForSeconds(delayBeforeSave);
        PlayOnSDK.SetUnityEditorDPI(96);
        if(this!=null){
            if(_canvas==null){
                if(this.transform.parent != null) _canvas = this.transform.parent.GetComponent<Canvas>();
            }
            if(_canvas!=null){
                if (_canvas.pixelRect.width >= 1440)
                {
                    PlayOnSDK.SetUnityEditorDPI(440);
                }
                else if (_canvas.pixelRect.width >= 1080)
                {
                    PlayOnSDK.SetUnityEditorDPI(323);
                }
                else if (_canvas.pixelRect.width >= 720)
                {
                    PlayOnSDK.SetUnityEditorDPI(252);
                }
                else if (_canvas.pixelRect.width >= 480)
                {
                    PlayOnSDK.SetUnityEditorDPI(170);
                }

                minPXSize = (((70 + 0.5f) * ((float) PlayOnSDK.GetUnityEditorDPI() / 160f)) / _canvas.scaleFactor);
                maxPXSize = (((120 + 0.5f) * ((float) PlayOnSDK.GetUnityEditorDPI() / 160f)) / _canvas.scaleFactor);
                if (saved)
                    SetSavedSize(savedSize);
                if (_canvas.pixelRect.width != savedWindowSizeDelta.x || _canvas.pixelRect.height != savedWindowSizeDelta.y)
                {
                    savedWindowSizeDelta = new Vector2(_canvas.pixelRect.width, _canvas.pixelRect.height);
                    FixSize();
                }

                if (!saved)
                    SaveSizeAndPosition();
             
                inited = true;  
            }
        }
        yield return null;
    }


    private float GetSizeInDP(float vectorSize) {
        var size = savedSize;
        if(_canvas.scaleFactor != 1 && inited)
            size = vectorSize/ (((float) PlayOnSDK.GetUnityEditorDPI() / 160f)/_canvas.scaleFactor) - 0.5f;
        return size;
    }
    private void FixSize() {
        _transform.localScale = Vector3.one;
        if (_transform.sizeDelta.x > maxPXSize)
        {
            _transform.sizeDelta = new Vector2(maxPXSize, maxPXSize);
        }

        if (_transform.sizeDelta.x < minPXSize)
        {
            _transform.sizeDelta = new Vector2(minPXSize, minPXSize);
        }

        _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, _transform.sizeDelta.x);
    }


    public void FixAnchors() {
        if (savedAnchorMax != _transform.anchorMax)
        {
            SetAnchorMin(_transform.anchorMax);
            savedAnchorMin = _transform.anchorMin;
            savedAnchorMax = _transform.anchorMax;
        }
        else if (savedAnchorMin != _transform.anchorMin)
        {
            SetAnchorMax(_transform.anchorMin);
            savedAnchorMin = _transform.anchorMin;
            savedAnchorMax = _transform.anchorMax;
        }
    }

    private bool anchoredChangedFromCode = false;

    private void SetAnchorMin(Vector2 anch) {
        _transform.anchorMin = anch;
        anchoredChangedFromCode = true;
    }

    private void SetAnchorMax(Vector2 anch) {
        _transform.anchorMax = anch;
        anchoredChangedFromCode = true;
    }

    private void SetSavedSize(float pos) {
        var size = (((pos + 0.5f) * ((float) PlayOnSDK.GetUnityEditorDPI() / 160f)) / _canvas.scaleFactor);
        _transform.sizeDelta = new Vector2(size, size);
    }

    private void SaveSizeAndPosition() {
        savedPosition = _transform.position;
        savedSize = GetSizeInDP(_transform.sizeDelta.x);
        saved = true;
    }

    private bool isAnchoredChangedFromCode() {
        if (anchoredChangedFromCode == true)
        {
            anchoredChangedFromCode = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Freeze() {
        _transform.rotation = Quaternion.identity;
        _transform.localScale = Vector3.one;
    }

#endif

    public RectTransform GetRectTransform() {
        if(_transform == null) _transform = (RectTransform) transform;
        return _transform;
    }

    public Canvas GetCanvas() {
        if (_canvas == null)
        {
            _canvas = this.transform.parent.GetComponent<Canvas>();
            return _canvas;
        }
        else
        {
            return _canvas;
        }
    }

    protected override void Start() {
        if (Application.isPlaying)
            this.gameObject.SetActive(false);
    }
}