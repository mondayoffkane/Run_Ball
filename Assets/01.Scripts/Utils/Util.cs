using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Lofelt.NiceVibrations;

public class Util
{

    private static Dictionary<float, WaitForSeconds> waitDic = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds WaitGet(float waitSec)
    {
        if (waitDic.TryGetValue(waitSec, out WaitForSeconds waittime)) return waittime;
        return waitDic[waitSec] = new WaitForSeconds(waitSec);
    }
    public static WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }

    public static T DeepCopy<T>(T obj)
    {
        using (var stream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0;

            return (T)formatter.Deserialize(stream);
        }
    }

    public static T StringToEnum<T>(string name) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), name);
    }

    ///<summary>분산랜덤</summary>
    /// <param name="variance">클수록 뾰족해짐! 최소값 기본값이 1</param>
    public static int DistributionRandom(int middleValue, float range, int variance = 1)
    {
        variance = Mathf.Max(1, variance);
        variance = (int)Math.Pow(10, variance);

        float u = UnityEngine.Random.Range(0.0f, 1.0f - Mathf.Epsilon);
        int ret = (int)(Mathf.Log(u / (1 - u), variance) * range + middleValue);

        while (ret < middleValue - range || ret > middleValue + range)
        {
            u = UnityEngine.Random.Range(0.0f, 1.0f - Mathf.Epsilon);
            ret = (int)(Mathf.Log(u / (1 - u), variance) * range + middleValue);
        }
        return ret;
    }

    /// <param name="power">1 lightimpact   //  2 MediumImpact //  3heavyimpact</param>
    public static void Haptic(int power = 1)
    {
        if (!Managers.Data.UseHaptic) return;

        if (power == 2)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        else if (power == 3)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        else
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

    }

    public static Vector3 UiToRealPos(Vector2 uiPos)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(uiPos.x, uiPos.y, (Camera.main.nearClipPlane + Camera.main.farClipPlane) * 0.5f));
    }
    public static Vector2 RealPosToUi(Vector3 realPos)
    {
        return Camera.main.WorldToScreenPoint(realPos);
    }

    public static void ParticleStart(GameObject particle)
    {
        particle.GetComponent<ParticleSystem>()?.PlayAllParticle();
    }

    public static void ParticleStop(GameObject particle)
    {
        particle.GetComponent<ParticleSystem>()?.StopAllParticle();
    }
}