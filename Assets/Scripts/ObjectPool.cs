using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [Header("Cài đặt")]
    public GameObject bulletPrefab;
    public int poolAmount = 50;

    // Danh sách chứa đạn
    private List<GameObject> pooledObjects;

    void Awake() 
    {
        
        if (instance == null) instance = this;

        
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        
        if (pooledObjects == null)
        {
            return null;
        }

        
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            
            if (pooledObjects[i] == null) continue;

            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}