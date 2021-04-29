using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSpawner : MonoBehaviour
{
    public List<GameObject> assetsToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Spawn ()
    {
        GameObject asset = assetsToSpawn[Random.Range (0, assetsToSpawn.Count)];
        Instantiate (asset, transform.position, asset.transform.rotation);
    }
}
