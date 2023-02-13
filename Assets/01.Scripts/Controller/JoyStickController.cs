using UnityEngine;
using UnityEngine.UI;
public enum JoyStickMethod
{
    DoNotUse,
    Fixed,
    HardFixed,
    Follow,
    SlowFollow,
}
public class JoyStickController : MonoBehaviour
{
    public JoyStickMethod joyStickMethod;
    public float JoyStickBound;

    private RectTransform _joystick;
    private RectTransform _joystickHandle;
    private Image _joystickImage;
    private Image _joystickHandleImage;

    private Vector3 _oriPos;

    private bool isReady = false;

    public System.Action DownAction;
    public System.Action<Vector2> JoystickMoveAction;
    public System.Action UpAction;

    public Vector3 Dir;

    public void Init(Image joystick)
    {
        _joystick = joystick.GetComponent<RectTransform>();
        _joystickHandle = _joystick.GetChild(0).GetComponent<RectTransform>();
        _joystickImage = joystick;
        _joystickHandleImage = _joystickHandle.GetComponent<Image>();
        _joystickImage.enabled = false;
        _joystickHandleImage.enabled = false;

        switch (joyStickMethod)
        {
            case JoyStickMethod.DoNotUse:
                _joystick.gameObject.SetActive(false);
                break;
            case JoyStickMethod.Fixed:
                break;
            case JoyStickMethod.HardFixed:
                _joystickImage.enabled = true;
                _joystickHandleImage.enabled = true;
                break;
            case JoyStickMethod.Follow:
                break;
            case JoyStickMethod.SlowFollow:
                break;
        }
        isReady = true;

        Managers.Game.joyStickController = this;
    }

    private void Update()
    {
        if (!isReady) return;

        switch (joyStickMethod)
        {
            case JoyStickMethod.DoNotUse:
                return;
            case JoyStickMethod.Fixed:
                if (Input.GetMouseButtonDown(0))
                {
                    _joystickImage.enabled = true;
                    _joystickHandleImage.enabled = true;

                    _joystick.anchoredPosition = Input.mousePosition * 2688f / Screen.height;
                    _joystickHandle.anchoredPosition = Vector2.zero;
                    _oriPos = _joystick.anchoredPosition;

                    DownAction?.Invoke();
                }
                else if (Input.GetMouseButton(0))
                {
                    _joystickHandle.anchoredPosition = Input.mousePosition * 2688f / Screen.height - _oriPos;
                    if (_joystickHandle.anchoredPosition.magnitude > JoyStickBound)
                    {
                        _joystickHandle.anchoredPosition = _joystickHandle.anchoredPosition.normalized * JoyStickBound;
                    }
                    Dir = (_joystickHandle.position - _joystick.position).normalized;
                    JoystickMoveAction?.Invoke(_joystickHandle.anchoredPosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _joystickImage.enabled = false;
                    _joystickHandleImage.enabled = false;
                    Dir = Vector3.zero;
                    UpAction?.Invoke();
                }
                break;
            case JoyStickMethod.HardFixed:
                if (Input.GetMouseButtonDown(0))
                {
                    DownAction?.Invoke();
                }
                else if (Input.GetMouseButton(0))
                {
                    _joystickHandle.anchoredPosition = (Vector2)(Input.mousePosition * 2688f / Screen.height) - _joystick.anchoredPosition;
                    if (_joystickHandle.anchoredPosition.magnitude > JoyStickBound)
                    {
                        _joystickHandle.anchoredPosition = _joystickHandle.anchoredPosition.normalized * JoyStickBound;
                    }
                    JoystickMoveAction?.Invoke(_joystickHandle.anchoredPosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _joystickHandle.anchoredPosition = Vector2.zero;
                }
                break;
            case JoyStickMethod.Follow:
                if (Input.GetMouseButtonDown(0))
                {
                    _joystickImage.enabled = true;
                    _joystickHandleImage.enabled = true;

                    _joystick.anchoredPosition = Input.mousePosition * 2688f / Screen.height;
                    _joystickHandle.anchoredPosition = Vector2.zero;
                    _oriPos = _joystick.anchoredPosition;

                    DownAction?.Invoke();
                }
                else if (Input.GetMouseButton(0))
                {
                    _joystickHandle.anchoredPosition = Input.mousePosition * 2688f / Screen.height - _oriPos;
                    if (_joystickHandle.anchoredPosition.magnitude > JoyStickBound)
                    {
                        _joystick.anchoredPosition = (Vector2)_oriPos + _joystickHandle.anchoredPosition - JoyStickBound * _joystickHandle.anchoredPosition.normalized;
                        _joystickHandle.anchoredPosition = _joystickHandle.anchoredPosition.normalized * JoyStickBound;
                        _oriPos = _joystick.anchoredPosition;
                    }
                    JoystickMoveAction?.Invoke(_joystickHandle.anchoredPosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _joystickImage.enabled = false;
                    _joystickHandleImage.enabled = false;
                    UpAction?.Invoke();
                }
                break;
            case JoyStickMethod.SlowFollow:
                if (Input.GetMouseButtonDown(0))
                {
                    _joystickImage.enabled = true;
                    _joystickHandleImage.enabled = true;

                    _joystick.anchoredPosition = Input.mousePosition * 2688f / Screen.height;
                    _joystickHandle.anchoredPosition = Vector2.zero;
                    _oriPos = _joystick.anchoredPosition;

                    DownAction?.Invoke();
                }
                else if (Input.GetMouseButton(0))
                {
                    _joystickHandle.anchoredPosition = Input.mousePosition * 2688f / Screen.height - _oriPos;
                    if (_joystickHandle.anchoredPosition.magnitude > JoyStickBound)
                    {
                        _joystick.anchoredPosition = Vector3.Lerp(_joystick.anchoredPosition, (Vector2)_oriPos + _joystickHandle.anchoredPosition - JoyStickBound * _joystickHandle.anchoredPosition.normalized, Time.deltaTime);
                        _oriPos = _joystick.anchoredPosition;
                    }
                    JoystickMoveAction?.Invoke(_joystickHandle.anchoredPosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _joystickImage.enabled = false;
                    _joystickHandleImage.enabled = false;
                    UpAction?.Invoke();
                }
                break;
        }
    }


    public void AddDownEvent(System.Action action)
    {
        DownAction -= action;
        DownAction += action;
    }
    public void AddMoveEvent(System.Action<Vector2> action)
    {
        JoystickMoveAction -= action;
        JoystickMoveAction += action;
    }
    public void AddUpEvent(System.Action action)
    {
        UpAction -= action;
        UpAction += action;
    }

}
