using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{

    public GameObject Child;

    public List<GameObject> Child_List;


    public bool isPlayer = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddChild();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                RemoveChild();
            }
        }

    }



    public void AddChild(int _Count = 1, GameObject _copyObj = null)
    {
        if (_copyObj == null)
        {
            _copyObj = Child;
        }

        for (int i = 0; i < _Count; i++)
        {

            GameObject _obj = Instantiate(_copyObj);
            _obj.GetComponent<Child>().Parent = this;
            _obj.GetComponent<Child>().Init();

            _obj.transform.position = transform.position + new Vector3(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f));
            Child_List.Add(_obj);
        }
    }

    public void RemoveChild(GameObject _child = null)
    {

        GameObject _obj = _child;
        if (_child == null)
        {
            _obj = Child_List[Child_List.Count - 1];
        }

        Child_List.Remove(_obj);
        Destroy(_obj);

    }

    public void MergeChild()
    {

    }

    

}
