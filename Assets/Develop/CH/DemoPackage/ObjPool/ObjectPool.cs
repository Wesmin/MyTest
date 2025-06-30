using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // 预制体
    private int initialPoolSize = 1; // 初始对象池大小
    private List<GameObject> objectList = new List<GameObject>();

    void Start()
    {
        InitializeObjectPool();
    }

    void InitializeObjectPool()
    {
        // 初始化对象池
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectList.Add(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        // 从对象池获取对象
        foreach (GameObject obj in objectList)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // 如果对象池中没有可用对象，动态增加对象池的大小
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);
        objectList.Add(newObj);
        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        // 将对象放回对象池
        obj.SetActive(false);
    }
}
