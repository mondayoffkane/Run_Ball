using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Ring : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball _ball = other.GetComponent<Ball>();
            //Transform _floating = Managers.Pool.Pop(Resources.Load<GameObject>("Floating_Money"), Managers.Game.transform).transform;
            //_floating.position = new Vector3(other.transform.position.x, other.transform.position.y, -1);



            //Text _floatingText;
            //_floatingText = _floating.transform.GetComponentInChildren<Text>();
            //_floatingText.text = $"$ {GameManager.ToCurrencyString(_ball.Price)}";

            //_floatingText.color = new Vector4(
            //        _floatingText.color.r
            //        , _floatingText.color.g
            //        , _floatingText.color.b
            //        , 1f);

            //DOTween.Sequence().Append(_floating.DOMoveY(_floating.position.y + 1f, 0.5f).SetEase(Ease.Linear))
            //    .Join(_floatingText.DOColor(new Vector4(
            //        _floatingText.color.r
            //        , _floatingText.color.g
            //        , _floatingText.color.b
            //        , 0f), 0.5f)).SetEase(Ease.Linear)
            //        .OnComplete(() => Managers.Pool.Push(_floating.GetComponent<Poolable>()));



            //Managers.Game.AddMoney(_ball.Price);

            Managers.Game.FloatingTextFunc(_ball.Price, transform);

        }
    }
}
