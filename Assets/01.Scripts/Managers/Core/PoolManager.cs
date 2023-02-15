using System.Collections.Generic;
using UnityEngine;
public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.SetParent(Root);
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            // DontDestroyOnLoad 해제 용도
            if (parent == null)
                poolable.transform.SetParent(Managers.Scene.CurrentScene.transform);

            poolable.transform.SetParent(parent);
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    ///<summary>매니저 만들때 호출됨</summary>
    public void Init()
    {
        _root = new GameObject { name = "@Pool_Root" }.transform;
        Object.DontDestroyOnLoad(_root);
    }
    ///<summary>Pool을 만들고 Dic에 추가함</summary>
    public void CreatePool(GameObject original, int count = 1)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.SetParent(_root);

        _pool.Add(original.name, pool);
    }
    ///<summary>Pool에 넣기 ( 반환 ) </summary>
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (!_pool.ContainsKey(name))
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }
    ///<summary>Pool에서 빼기 ( 사용 ) </summary>
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (!_pool.ContainsKey(original.name))
            CreatePool(original);

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (!_pool.ContainsKey(name))
            return null;
        return _pool[name].Original;
    }

    ///<summary>씬 전환시 호출 됨</summary>
    public void Clear()
    {
        if (_root == null) return;
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
