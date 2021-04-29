using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFromPool : MonoBehaviour
{
    PoolManager poolManager;
    public enum SpawnMode { Random, Fixed};

    public string poolTag;
    public SpawnMode spawnMode;
    public int maxSpawnedObjects; 
    public Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        poolManager = PoolManager.Instance;
        if (spawnMode == SpawnMode.Random)
            maxSpawnedObjects = Random.Range (1, maxSpawnedObjects);
    }


    public void SpawnObjects ()
    {
        poolManager.SpawnObjectRepeating (maxSpawnedObjects, poolTag, spawnPoint.position, spawnPoint.rotation);       
    }
}
