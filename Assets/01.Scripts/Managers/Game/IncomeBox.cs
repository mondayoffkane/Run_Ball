using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IncomeBox : MonoBehaviour
{

    public float Base_Scope;
    public float Add_Scope;

    [SerializeField] float Total_Scope;
    [SerializeField] Text _text;

    private void Start()
    {
        _text = transform.GetComponentInChildren<Text>();

        Upgrade_Income();
    }


    public void Upgrade_Income(int _incomeLevel = 0)
    {
        Total_Scope = Base_Scope + Add_Scope * _incomeLevel;
        _text.text = $"X {Total_Scope}";
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball _ball = other.GetComponent<Ball>();
            Transform _floating = Managers.Pool.Pop(Resources.Load<GameObject>("Floating_Money"), transform).transform;
            _floating.position = new Vector3(other.transform.position.x, other.transform.position.y, -1);



            Text _floatingText;
            _floatingText = _floating.transform.GetComponentInChildren<Text>();
            _floatingText.text = $"$ {_ball.Price * Total_Scope}";

            _floatingText.color = new Vector4(
                    _floatingText.color.r
                    , _floatingText.color.g
                    , _floatingText.color.b
                    , 1f);

            DOTween.Sequence().Append(_floating.DOMoveY(_floating.position.y + 1f, 0.5f).SetEase(Ease.Linear))
                .Join(_floatingText.DOColor(new Vector4(
                    _floatingText.color.r
                    , _floatingText.color.g
                    , _floatingText.color.b
                    , 0f), 0.5f)).SetEase(Ease.Linear)
                    .OnComplete(() => Managers.Pool.Push(_floating.GetComponent<Poolable>()));

            Managers.Game.AddMoney((double)(_ball.Price * Total_Scope));          
        }
    }
}
