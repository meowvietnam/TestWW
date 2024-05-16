using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolManager : MonoBehaviour
{
   
    public List<ObjectPool> listPool = new();
    public GameObject poolObject;
    protected virtual void Awake()
    {
        GetComponent();

    }
    // Start is called before the first frame update

    private void Start()
    {

    }
    void GetComponent()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            listPool.Add(transform.GetChild(i).GetComponent<ObjectPool>());
        }
    }
    public virtual void ActivePoolObject(int index)
    {
        poolObject = listPool[index].GetPoolObject();
      

    }
    public void DestroyAfterEvent(GameObject obj, int index)
    {
        obj.transform.SetParent(gameObject.transform.GetChild(index));
        obj.SetActive(false);
    }


}
