using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomContentsController : MonoBehaviour
{
    List<AssetSpawner> baseBodySpawnPoints;
    List<AssetSpawner> enemySpawnPoints;

    public List<AssetSpawner> roomAssetSpawnPoints; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnRoomAssets ()
    {
        foreach(AssetSpawner sp in roomAssetSpawnPoints)
        {
            sp.Spawn ();
        }
    }
}
