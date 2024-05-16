using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    [SerializeField] public GameObject prefab;
    [SerializeField] private int size;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        CreateObject(size);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateObject(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
          GameObject thisObj =  Instantiate(prefab, gameObject.transform);
          thisObj.SetActive(false);
        }    
    }   
    public GameObject GetPoolObject()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        GameObject objCreateAdd = Instantiate(prefab,gameObject.transform);
        return objCreateAdd;
    }    
}
