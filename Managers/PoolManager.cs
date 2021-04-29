using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    struct Pool
    {
        public string tag;
        public GameObject prefab;
        public float size;
    }

    #region Singleton

    public static PoolManager Instance;
    private void Awake ()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy (this);
    }

    #endregion

    [SerializeField]
    List<Pool> pools;
    Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>> ();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject> ();
            for(int i=0; i<pool.size; i++)
            {
                GameObject spawnedObject =  Instantiate (pool.prefab);
                spawnedObject.SetActive (false);
                objectPool.Enqueue (spawnedObject);
            }
            poolDict.Add (pool.tag, objectPool);
        }
    }

    public GameObject SpawnObject(string tag, Vector3 pos, Quaternion rot, bool autoEnqueue = false)
    {
        if (!poolDict.ContainsKey (tag))
        {
            Debug.LogWarning ("Pool with tag " + tag + " doesnt exist");
            return null;
        }
        else if (poolDict[tag].Count <= 0)
        {
            foreach (Pool pool in pools)
            {
                if(pool.tag == tag)
                {
                    GameObject spawnedObject = Instantiate (pool.prefab);
                    poolDict[tag].Enqueue (spawnedObject);
                    break;
                }
            }
        }
        GameObject obj = poolDict[tag].Dequeue ();
        obj.SetActive (true);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        if(autoEnqueue)
            poolDict[tag].Enqueue (obj);

        ISpawnableObject spawnableInterface = obj.GetComponent<ISpawnableObject> ();
        if (spawnableInterface != null)
            spawnableInterface.OnSpawn (tag);

        return obj;
    }

    public void SpawnObjectRepeating (int cuantity, string tag, Vector3 pos, Quaternion rot, bool autoEnqueue = false)
    {
        if (!poolDict.ContainsKey (tag))
        {
            Debug.LogWarning ("Pool with tag " + tag + " doesnt exist");
            return;
        }
        for (int i = 0; i < cuantity; i++)
        {
            if (poolDict[tag].Count <= 0)
            {
                foreach (Pool pool in pools)
                {
                    if (pool.tag == tag)
                    {
                        GameObject spawnedObject = Instantiate (pool.prefab);
                        poolDict[tag].Enqueue (spawnedObject);
                        break;
                    }
                }
            }
            GameObject obj = poolDict[tag].Dequeue ();
            obj.SetActive (true);
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            if (autoEnqueue)
                poolDict[tag].Enqueue (obj);

            ISpawnableObject spawnableInterface = obj.GetComponent<ISpawnableObject> ();
            if (spawnableInterface != null)
                spawnableInterface.OnSpawn (tag);
        }
    }

    public void AddToPool (string tag, GameObject obj)
    {
        poolDict[tag].Enqueue (obj);
    }
}
