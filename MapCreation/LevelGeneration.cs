using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[System.Serializable]
public class RoomDoors
{
    public RoomDoors (Room _parent, Vector2 _coords)
    {
        parentRoom = _parent;
        coordinates = _coords;
        availabeDoors = new List<Vector2>();
        usedDoors = new List<Vector2> ();
    }
    public RoomDoors (Room _parent, Vector2 _coords, List<Vector2> _doors)
    {
        parentRoom = _parent;
        coordinates = _coords;
        availabeDoors = _doors;
        usedDoors = new List<Vector2> ();
    }
    Room parentRoom;
    Vector2 coordinates;
    List<Vector2> availabeDoors;
    List<Vector2> usedDoors;
    public Vector2 GetCoords ()
    {
        return coordinates;
    }
    public void AddDoor (Vector2 _doorCoord)
    {
        availabeDoors.Add (_doorCoord);
    }
    public int GetDoorsLeft ()
    {
        return availabeDoors.Count;
    }
    public Vector2 GetRandomDoor ()
    {
        int rnd = Random.Range (0, availabeDoors.Count);
        Vector2 door = availabeDoors[rnd];
        return door;
    }
    public void UseDoor (Vector2 door)
    {
        availabeDoors.Remove (door);
        usedDoors.Add (door);
    }
    public List<Vector2> GetUsedDoors ()
    {
        return usedDoors;
    }
}
public class LevelGeneration : MonoBehaviour
{
    public static LevelGeneration Instance;

    [SerializeField]
    GameObject doorPrefab;
    [SerializeField]
    GameObject intialRoomPrefab;
    [SerializeField]
    GameObject firstRoom;
    [SerializeField]
    List<GameObject> irrRooms;
    [SerializeField]
    int numberOfRooms;
    [SerializeField]
    Vector3 startingRoomPos;

    List<Room> generatedRooms = new List<Room> ();
    List<RoomDoors> levelRooms = new List<RoomDoors>();
    List<WallCollisionChecker> levelWalls = new List<WallCollisionChecker> ();

    public delegate void FinishLevelGeneration ();
    public event FinishLevelGeneration OnFinish;

    private void Awake ()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy (this);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine (GenerateIrregularRoomsSlowly ());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GenerateIrregularRoomsSlowly ()
    {
        List<List<RoomDoors>> roomDoors = new List<List<RoomDoors>> ();
        List<Vector2> gridPositions = new List<Vector2> ();

        Vector3 roomPos = Vector3.zero;

        firstRoom = Instantiate (intialRoomPrefab, startingRoomPos, Quaternion.Euler (Vector3.zero));
        Room initRoomC = firstRoom.GetComponent<Room> ();
        OnFinish += initRoomC.ForceOpenRoom;
        initRoomC.StartIrregularRoom (roomPos);

        foreach(Vector2 tile in initRoomC.GetRoomTiles ())
        {
            gridPositions.Add (tile);
        }

        roomDoors.Add (initRoomC.GetTilesInfo ());
        generatedRooms.Add (initRoomC);
        yield return null;
        levelWalls.Clear ();
        
        int i = 1;
        int securityLoop = 0;
        float maxDistanceToCenter = 0;
        while (roomDoors.Count > 0 && i < numberOfRooms && securityLoop < numberOfRooms * 200)
        {
            int rnd = Random.Range (0, roomDoors.Count);
            int rnd2 = Random.Range (0, roomDoors[rnd].Count);
            RoomDoors tileSelected = roomDoors[rnd][rnd2];
            float distance = tileSelected.GetCoords ().magnitude;
            bool roomAccepted = CheckPercentaje (distance, maxDistanceToCenter);
            if (roomAccepted)
            {
                if (tileSelected.GetDoorsLeft () > 0)
                {
                    Vector2 doorCoords = tileSelected.GetRandomDoor ();
                    Vector2 newRoomPos = doorCoords;
                    roomPos.x = newRoomPos.x;
                    roomPos.z = newRoomPos.y;

                    if (!gridPositions.Contains (newRoomPos))
                    {
                        //Debug.Log (roomPos.x + " : " + roomPos.z);

                        GameObject room = Instantiate (irrRooms[Random.Range (0, irrRooms.Count)], startingRoomPos + new Vector3 (newRoomPos.x * 16, 0, newRoomPos.y * 16), Quaternion.Euler(Vector3.zero));
                        Room roomC = room.GetComponent<Room> ();
                        roomC.StartIrregularRoom (newRoomPos);
                        bool contains = false;
                        int spaceFilledCounter = 0;
                        //Check recursively if a tile of the room is colliding with anything
                        List<Vector2> tiles = roomC.GetRoomTiles ();
                        Vector2 movement = Vector2.zero;
                        do
                        {
                            movement = tiles[0] - tiles[spaceFilledCounter];
                            foreach (Vector2 tile in tiles)
                            {
                                if (gridPositions.Contains (tile+movement))
                                {
                                    contains = true;
                                    break;
                                }
                                else
                                {
                                    contains = false;
                                }
                            }
                            spaceFilledCounter++;
                        } while (contains&& spaceFilledCounter < roomC.GetRoomTiles ().Count);

                        if (contains)
                            Destroy (room);
                        else
                        {
                            tileSelected.UseDoor (doorCoords);
                            
                            roomC.MoveRoomTiles (movement);
                            Vector2 newRootTile = roomC.GetRoomTiles ()[0];
                            room.transform.position = startingRoomPos + new Vector3 (newRootTile.x * 16, 0, newRootTile.y * 16);
                            foreach (Vector2 tile in roomC.GetRoomTiles ())
                            {
                                gridPositions.Add (tile);
                            }
                            roomDoors.Add (roomC.GetTilesInfo ());
                            roomC.AddUsedDoor (doorCoords, tileSelected.GetCoords ());                          

                            if (newRoomPos.magnitude > maxDistanceToCenter)
                                maxDistanceToCenter = newRoomPos.magnitude;
                            i++;
                        }

                        if(room)
                        {
                            levelRooms.Add (tileSelected);
                            generatedRooms.Add (roomC);
                            yield return new WaitForSeconds(0.05f);
                            foreach (WallCollisionChecker wall in levelWalls)
                            {
                                if (wall)
                                {
                                    wall.CheckCollision ();
                                }
                            }
                            levelWalls.Clear ();
                        }
                            
                    }
                }
                else
                {
                    roomDoors.RemoveAt (rnd);
                }
               
            }
            else
            {
                securityLoop++;
            }
        }
        Debug.Log ("LoopSecurity: " + securityLoop);
        StartCoroutine (GenerateDoors ());
    }

    IEnumerator GenerateDoors ()
    {
        foreach(RoomDoors room in levelRooms)
        {
            Vector3 roomCords = startingRoomPos;
            roomCords.x += room.GetCoords ().x * 16;
            roomCords.y += 0;
            roomCords.z += room.GetCoords ().y * 16;
            Vector3 roomOffset;
            roomOffset.x = 8;
            roomOffset.y = 2.5f;
            roomOffset.z = 6;
            foreach (Vector2 door in room.GetUsedDoors ())
            {
                Vector3 doorCords = startingRoomPos;
                doorCords.x += door.x * 16;
                doorCords.y += 0;
                doorCords.z += door.y * 16;

                Debug.DrawRay (roomCords + roomOffset, (doorCords - roomCords).normalized*10, Color.red, 10);
                if(Physics.Raycast(roomCords + roomOffset, (doorCords - roomCords), out RaycastHit hit, 10, 1<<7 ))
                {
                    //Debug.Log("Detected: " + hit.collider.gameObject.name);
                    if (hit.collider.tag == "ExtWall")
                    {
                        GameObject doorObject = Instantiate (doorPrefab, hit.collider.gameObject.transform.position - new Vector3(0,1.7f,0), hit.collider.gameObject.transform.rotation);
                        doorObject.transform.localScale = new Vector3 (147, 147, 147);
                        doorObject.transform.position+= doorObject.transform.right * 0.1f;
                        Destroy (hit.collider.gameObject);
                    }
                }
                else
                {
                    //Debug.LogError ("No door seen");
                }
                yield return new WaitForSeconds (0.1f);
            }
        }
        foreach(Room room in generatedRooms)
        {
            room.SetPhysicDoors ();
        }
        OnFinish?.Invoke ();
    }

    public GameObject GetRoomTileDoor (Vector2 _roomCoords, Vector2 _doorCoords ) 
    {
        GameObject door = null;

        Vector3 roomCords = startingRoomPos;
        roomCords.x += _roomCoords.x * 16;
        roomCords.y += 0;
        roomCords.z += _roomCoords.y * 16;
        Vector3 roomOffset;
        roomOffset.x = 8;
        roomOffset.y = 2.5f;
        roomOffset.z = 6;
        Vector3 doorCords = startingRoomPos;
        doorCords.x += _doorCoords.x * 16;
        doorCords.y += 0;
        doorCords.z += _doorCoords.y * 16;

        
        if (Physics.Raycast (roomCords + roomOffset, (doorCords - roomCords), out RaycastHit hit, 10, 1 << 10))
        {
            door = hit.collider.gameObject;
            Debug.DrawRay (roomCords + roomOffset, (doorCords - roomCords).normalized * 10, Color.green, 10);
        }
        else
        {
            Debug.LogError ("DoorNotFound");
            Debug.DrawRay (roomCords + roomOffset, (doorCords - roomCords).normalized * 10, Color.blue, 10);
        }

        return door;
    }

    //RoomDoors GetRoomByCoords (Vector2 _coords)
    //{
    //    foreach (List<RoomDoors> roomList in roomDoors)
    //    {
    //        foreach(RoomDoors room in roomList)
    //        {
    //            Debug.Log (room.GetCoords ());
    //            if (room.GetCoords () == _coords)
    //                return room;
    //        }          
    //    }
    //    return null;
    //}

    bool CheckPercentaje(float distance, float maxDistance)
    {
        if (maxDistance == 0)
            return  true;
        if (distance == 0)
            distance += 1;
        bool lucky = false;
        float rnd = Random.Range (0f, 1f);
        float percentage = (float)(distance) / (float)(maxDistance);
        if ((1 - percentage) < rnd*0.9f)
            lucky = true;
        return lucky;
    }
    public void AddLevelWall(WallCollisionChecker wall)
    {       
        levelWalls.Add (wall);
    }
}
