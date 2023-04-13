using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ClearGate : MonoBehaviour
{

    public double clearMoney = 100;


    public bool isOpen = false;

    public float _time = 2f;

    Text _countText;

    double Money = 0d;

    public bool isHide = true;
    public GameObject Effect;
    public GameObject GiftBox;
    bool isFirst = true;

    private void Start()
    {
        if (isHide)
            gameObject.SetActive(false);


        Effect.SetActive(false);

        DOTween.Sequence().Append(transform.DOScale(Vector3.one * 0.8f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo))
            .Join(transform.DORotate(Vector3.up * 360f, 4f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart));




    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            /*  if (isOpen == false)
              {
                  Money = collision.transform.GetComponent<Ball>().Price;
                  Transform _floating = Managers.Pool.Pop(Resources.Load<GameObject>("Floating_Money"), Managers.Game.transform).transform;
                  _floating.position = new Vector3(collision.transform.position.x, collision.transform.position.y, -1);

                  Text _floatingText;
                  _floatingText = _floating.transform.GetComponentInChildren<Text>();
                  _floatingText.text = $"${GameManager.ToCurrencyString(Money)}";

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


                  Managers.Game.currentClearMoney += Money;
                  Managers.Game.AddMoney(Money);
                  if (Managers.Game.currentClearMoney > clearMoney)
                  {
                      Managers.Game.currentClearMoney = clearMoney;
                      StartCoroutine(OpenGate());
                  }
                  _countText.text = $"{(Managers.Game.currentClearMoney / clearMoney * 100d):0}%";





              }*/
            if (isFirst)
            {
                Effect.SetActive(true);
                GiftBox.SetActive(false);
                Managers.Game.StageClear(true);
                Managers.Game.isBonusReady = false;
                isFirst = false;

            }

        }
    }

    [Button]
    IEnumerator OpenGate()
    {
        yield return null;
        isOpen = true;

        // add dotween visual direction
        transform.DOShakeRotation(1f);

        yield return new WaitForSeconds(_time);
        Managers.Game.StageClear();

    }


}
