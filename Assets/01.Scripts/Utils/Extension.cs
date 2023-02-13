using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using DG.Tweening;
internal static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }
    public static void BindEvent(this GameObject go, Action action)
    {
        UI_Base.BindEvent(go, action);
    }
    public static void AddButtonEvent(this Button go, UnityEngine.Events.UnityAction action)
    {
        go.onClick.RemoveAllListeners();
        go.onClick.AddListener(action);

        // go.onClick.AddListener(delegate { Managers.Sound.Play($"Buttons/{buttonSound.ToString()}"); }); //버튼 소리


        //====여기 빼도됨 버튼 뽀잉 하고싶으면넣기=========
        go.transition = Selectable.Transition.Animation;
        Animator ani = go.gameObject.GetOrAddComponent<Animator>();
        RuntimeAnimatorController animatorController = Managers.Resource.Load<RuntimeAnimatorController>("ButtonAnimation");
        ani.runtimeAnimatorController = animatorController;
        //====여기 빼도됨 버튼 뽀잉 하고싶으면넣기=========
    }

    public async static void OnComplete<T>(this Task<T> task, Action action)
    {
        await task;
        action.Invoke();
    }
    public async static void OnComplete<T>(this Task<T> task, Action<T> action)
    {
        T ret = await task;
        action.Invoke(ret);
    }
    public static void Forget<T>(this Task<T> task)
    {
    }

    public static int Sum(this IEnumerable<int> array)
    {
        int sum = 0;
        foreach (int nums in array)
        {
            sum += nums;
        }
        return sum;
    }
    public static float Sum(this IEnumerable<float> array)
    {
        float sum = 0;
        foreach (float nums in array)
        {
            sum += nums;
        }
        return sum;
    }
    public static double Sum(this IEnumerable<double> array)
    {
        double sum = 0;
        foreach (double nums in array)
        {
            sum += nums;
        }
        return sum;
    }
    public static float Average(this IEnumerable<int> array)
    {
        int sum = 0;
        int count = 0;
        foreach (int nums in array)
        {
            sum += nums;
            count++;
        }
        return (float)sum / count;
    }
    public static float Average(this IEnumerable<float> array)
    {
        float sum = 0;
        int count = 0;
        foreach (float nums in array)
        {
            sum += nums;
            count++;
        }
        return sum / count;
    }
    public static double Average(this IEnumerable<double> array)
    {
        double sum = 0;
        int count = 0;
        foreach (double nums in array)
        {
            sum += nums;
            count++;
        }
        return sum / count;
    }
    public static void SetPositionX(this Transform tr, float x)
    {
        Vector3 pos = tr.position;
        pos.x = x;
        tr.position = pos;
    }
    public static void SetPositionY(this Transform tr, float y)
    {
        Vector3 pos = tr.position;
        pos.y = y;
        tr.position = pos;
    }
    public static void SetPositionZ(this Transform tr, float z)
    {
        Vector3 pos = tr.position;
        pos.z = z;
        tr.position = pos;
    }
    public static void SetPositionX(this RectTransform tr, float x)
    {
        Vector3 pos = tr.anchoredPosition3D;
        pos.x = x;
        tr.anchoredPosition3D = pos;
    }
    public static void SetPositionY(this RectTransform tr, float y)
    {
        Vector3 pos = tr.anchoredPosition3D;
        pos.y = y;
        tr.anchoredPosition3D = pos;
    }
    public static void SetPositionZ(this RectTransform tr, float z)
    {
        Vector3 pos = tr.anchoredPosition3D;
        pos.z = z;
        tr.anchoredPosition3D = pos;
    }
    public static void SetAlpha(this Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
    public static void PlayAllParticle(this ParticleSystem particle)
    {
        ParticleSystem[] particles = particle.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem part in particles)
        {
            part.Play();
        }
    }
    public static void StopAllParticle(this ParticleSystem particle)
    {
        ParticleSystem[] particles = particle.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem part in particles)
        {
            part.Stop();
        }
    }
}
