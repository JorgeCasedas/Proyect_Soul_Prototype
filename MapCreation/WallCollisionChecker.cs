using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionChecker : MonoBehaviour
{
    List<GameObject> wallsToDestroy = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //if(IsColliding())
            LevelGeneration.Instance.AddLevelWall (this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool IsColliding ()
    {
        Collider[] collisions;
        if ((collisions = Physics.OverlapSphere (transform.position, 0.1f)).Length > 0)
        {
            foreach (Collider col in collisions)
            {
                if (col.gameObject != gameObject)
                    wallsToDestroy.Add(col.gameObject);
            } 
        }
        if (wallsToDestroy.Count > 0)
            return true;
        else
            return false;
    }
    //public void CheckCollision ()
    //{
    //        foreach(GameObject go in wallsToDestroy)
    //        {
    //            if(go)
    //            Destroy(go);
    //        }
    //    Destroy (this);
    //}
    public void CheckCollision ()
    {
        Collider[] collisions;
        if ((collisions = Physics.OverlapSphere (transform.position, 0.1f)).Length > 0)
        {
            foreach (Collider col in collisions)
            {
                if (col.gameObject != gameObject)
                    Destroy(col.gameObject);
            }
        }
        Destroy (this);
    }
}
