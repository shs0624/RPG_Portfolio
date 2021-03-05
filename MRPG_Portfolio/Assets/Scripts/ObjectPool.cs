using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    public GameObject arrowPrefab;

    private Queue<GameObject> arrowPoolingQueue = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        InitalizeQueue(30);
    }

    private void InitalizeQueue(int count)
    {
        for(int i = 0; i < count; i++)
        {
            arrowPoolingQueue.Enqueue(InitializeObject());
        }
    }

    private GameObject InitializeObject()
    {
        GameObject obj = Instantiate(arrowPrefab);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }

    public GameObject GetArrowObj()
    {
        if (instance.arrowPoolingQueue.Count > 0)
        {
            var obj = instance.arrowPoolingQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }

        else
        {
            var newObj = instance.InitializeObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnArrowObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance.arrowPoolingQueue.Enqueue(obj);
    }
}
