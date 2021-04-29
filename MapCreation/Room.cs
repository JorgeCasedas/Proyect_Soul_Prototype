using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    Vector2[] possibleDoors = {new Vector2(1,0), new Vector2 (-1, 0), new Vector2 (0, 1), new Vector2 (0, -1) };
    List<RoomDoors> tilesInfo = new List<RoomDoors>();
    List<Vector2> roomTiles = new List<Vector2> ();
    List<GameObject> doorsPrefabs =  new List<GameObject>();
    int AIEnemies = 0;
    bool roomDone;
    public delegate void ClosingDoors ();
    public event ClosingDoors Closing;

    RoomContentsController roomController;
    // Start is called before the first frame update
    void Start()
    {
        roomController = gameObject.GetComponent<RoomContentsController> ();
        roomDone = false;
        LevelGeneration.Instance.OnFinish += FinishedSetingUpRoom;
        //StartIrregularRoom (Vector3.zero);
    }

    public void StartIrregularRoom (Vector2 posOffset)
    {
        SetRoomTiles (posOffset);
        GenerateIrregularRoomDoors ();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //private void OnDrawGizmos ()
    //{
    //    Vector3 centeredRoom = transform.GetChild (0).position;
    //    foreach (Vector2 door in irrDoors)
    //    {
    //        Vector3 doorPos;
    //        doorPos.x = centeredRoom.x + door.x*16;
    //        doorPos.y = centeredRoom.y;
    //        doorPos.z = centeredRoom.z + door.y * 16;
    //        Gizmos.DrawWireSphere (doorPos,1f);
    //    }
    //}
    void SetRoomTiles (Vector2 posOffset)
    {
        int i = 0;
        while (i < transform.childCount)
        {
            Vector3 childPos = transform.GetChild (i).localPosition;
            roomTiles.Add ((new Vector2 (childPos.x, childPos.z) / 16) + posOffset);
            i++;
        }
    }
    public void MoveRoomTiles (Vector2 movement)
    {
        for(int i=0;i<roomTiles.Count;i++)
        {
            roomTiles[i] += movement;
        }
        GenerateIrregularRoomDoors ();
    }
    public List<Vector2> GetRoomTiles ()
    {
        return roomTiles;
    }
    public void GenerateIrregularRoomDoors ()
    {
        if (tilesInfo.Count > 0)
        {
            tilesInfo.Clear ();
        }
        foreach(Vector2 room in roomTiles)
        {
            RoomDoors completedRoom =  new RoomDoors (this, room);
            foreach ( Vector2 door in possibleDoors)
            {
                Vector2 doorCoordinates = room + door;
                if (!roomTiles.Contains (doorCoordinates))
                {
                    completedRoom.AddDoor(doorCoordinates);
                }
            }
            tilesInfo.Add (completedRoom);
        }
    }
    public List<RoomDoors> GetTilesInfo ()
    {
        return tilesInfo;
    }
    public void AddUsedDoor(Vector2 tile, Vector2 prevTile)
    {
        foreach(RoomDoors tileInfo in tilesInfo)
        {
            if(tileInfo.GetCoords() == tile)
            {
                tileInfo.UseDoor (prevTile);
            }
        }
    }
    public void ForceOpenRoom ()
    {
        OpenDoors ();
    }
    public void SetPhysicDoors ()
    {
        foreach (RoomDoors tileInfo in tilesInfo)
        {
            Vector2 tileCoords = tileInfo.GetCoords ();
            List<Vector2> usedDoors = tileInfo.GetUsedDoors ();
            foreach(Vector2 doorPos in usedDoors)
            {
                doorsPrefabs.Add(LevelGeneration.Instance.GetRoomTileDoor (tileCoords, doorPos));
            }
        }
    }
    void FinishedSetingUpRoom ()
    {
        roomDone = true;
        if(roomController)
            roomController.SpawnRoomAssets ();
    }
    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "ownable" && !roomDone)
        {
            //if(other.isTrigger)
            //{
                BaseAI ai = other.gameObject.GetComponent<BaseAI> ();
                if (ai!=null )
                {
                    ai.SetRoom (this);
                    AIEnemies += 1;
                }
            //}   
        }
        else if (AIEnemies > 0)
        {
            CloseDoors ();
        }
        else if (AIEnemies <= 0)
        {
            OpenDoors ();
        }
    }
    public void RemoveEnemy ()
    {
        AIEnemies -= 1;
        if (AIEnemies <= 0)
        {
            OpenDoors ();
        }
    }
    void CloseDoors ()
    {
        Debug.Log ("CloseDoors");
        foreach(GameObject door in doorsPrefabs)
        {
            Animator anim = null;
            if (door.transform.childCount > 0)
                anim = door.transform.GetChild (0).GetComponent<Animator> ();
            else
                anim = door.transform.GetComponent<Animator> ();
            anim.SetBool ("Open", false);
        }
        Closing?.Invoke ();
    }
    void OpenDoors ()
    {
        foreach (GameObject door in doorsPrefabs)
        {
            Animator anim = null;
            if (door.transform.childCount>0)
                anim = door.transform.GetChild (0).GetComponent<Animator> ();
            else
                anim = door.transform.GetComponent<Animator> ();
            anim.SetBool ("Open", true);
        }
        Debug.Log ("OpenDoors");
    }
}
