using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.U2D;
public class ResourceManager
{
    ///<summary>Sprite는 Atlas로 만들면 아틀라스를 호출해야 하기 때문에 있는 Dic</summary>
    public readonly Dictionary<string, Sprite> SpriteInAtlas = new Dictionary<string, Sprite>();
    public void Init()
    {
        SpriteAtlas[] spriteAtlas = Resources.LoadAll<SpriteAtlas>("SpriteAtlas");
        foreach (SpriteAtlas atlas in spriteAtlas)
        {
            Sprite[] sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites);
            for (int j = 0; j < sprites.Length; j++)
            {
                SpriteInAtlas[sprites[j].name.Replace("(Clone)", "")] = sprites[j];
            }
        }
    }


    ///<summary>이미 로드 되었던 Asset을 담고있는 Dic</summary>
    private readonly Dictionary<string, object> loadedAsset = new Dictionary<string, object>();

    ///<summary>Resources.Load의 역할을 대신함</summary>
    public T Load<T>(string name) where T : Object
    {
        //풀링되어있는경우 풀 오브젝트를 줄것
        if (typeof(T) == typeof(GameObject))
        {
            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        if (loadedAsset.TryGetValue(name, out object value))
        {
            return value as T;
        }
        //그 외에 경우
        T ret = null;
        ret = Resources.Load<T>(name);
        loadedAsset[name] = ret;
        return ret;
    }
    ///<summary>Object.Instance의 역할을 대신함</summary>
    public GameObject Instantiate(string name, Transform parent = null)
    {
        GameObject original = Load<GameObject>(name);
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {name}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }
    public GameObject Instantiate(string name, float timer, Transform parent = null)
    {
        GameObject original = Load<GameObject>(name);
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {name}");
            return null;
        }
        if (original.GetComponent<Poolable>() != null)
        {
            Poolable poolable = Managers.Pool.Pop(original, parent);
            poolable.Timer(timer);
            return poolable.gameObject;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        Destroy(go, timer);
        return go;
    }
    public GameObject Instantiate(string name, Vector3 position, float timer = 0)
    {
        GameObject original = Load<GameObject>(name);
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {name}");
            return null;
        }
        if (original.GetComponent<Poolable>() != null)
        {
            Poolable poolable = Managers.Pool.Pop(original, null);
            if (timer != 0)
                poolable.Timer(timer);
            poolable.transform.position = position;
            return poolable.gameObject;
        }

        GameObject go = Object.Instantiate(original, null);
        go.name = original.name;
        if (timer != 0)
            Destroy(go, timer);
        go.transform.position = position;
        return go;
    }
    ///<summary>Object.Destroy 역할을 대신함 // timer에 변수 할당 시 timer(초) 후 반환 또는 파괴</summary>
    public async void Destroy(GameObject go, float timer = 0)
    {
        if (go == null)
            return;

        if (timer != 0)
        {
            await Task.Delay((int)(timer * 1000));
        }

        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }
        Object.Destroy(go);
    }

    ///<summary>씬 전환시 호출 // 그동안 사용한 에셋 모두 Release // 하나의 씬이 무겁다면 하는것이 좋지만 아니라면 안하는것이 나을듯</summary>
    public void Clear()
    {
        loadedAsset.Clear();
    }
}
