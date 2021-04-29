using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    public LayerMask layerMask;
    Room parentRoom;
    List<Vector3> optionalDirections = new List<Vector3> ();
    protected GameObject objetive;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Vector3 vec = transform.forward;
        float angle = 0;    
        while (angle < 360)
        {
            optionalDirections.Add (vec);
            angle += 22.5f;
            vec = Quaternion.Euler (0, angle, 0) * vec;
        }
    }
    public void SetObjetive ()
    {
        objetive = GameManager.Instance.character;
        parentRoom.Closing -= SetObjetive;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    protected Vector3 EvaluateDirection (Vector3 dir)
    {
        RaycastHit hit;
        Vector3 originPos = transform.position;
        originPos.y = 0.3f;
        dir = dir.normalized;
        dir.y = 0;
        Vector3 startingPoint = originPos + (dir * 0.15f);
        Vector3 optimalDirection = dir;
        List<Vector3> avoidVectors = new List<Vector3> ();
        float lastDot = 1;
        if (Physics.Raycast (startingPoint, dir, out hit, 1.5f, layerMask))
        {
            //Obstacle ahead
            Debug.DrawRay (startingPoint, dir, Color.red);
            lastDot = -1;
        }
        else
        {
            //No obstacle
            Debug.DrawRay (startingPoint, dir, Color.green);
        }
        foreach (Vector3 newDir in optionalDirections)
        {
            startingPoint = originPos + (newDir * 0.5f);
            if (Physics.Raycast (startingPoint, newDir, out hit, 1.5f, layerMask))
            {
                //Obstacle ahead
                Debug.DrawRay (startingPoint, newDir, Color.red);
                avoidVectors.Add (newDir);
            }
            else
            {
                //No obstacle
                Debug.DrawRay (startingPoint, newDir, Color.green);
                if (lastDot != 1)
                {
                    float dot = MyDot (dir, newDir);
                    if (dot > lastDot)
                    {
                        lastDot = dot;
                        optimalDirection = newDir;
                    }
                }
            }
        }
        foreach (Vector3 avoid in avoidVectors)
        {
            optimalDirection -= avoid / 2;
        }

        Debug.DrawRay (transform.position, optimalDirection, Color.white);
        return optimalDirection;
    }

    float MyDot (Vector3 a, Vector3 b)
    {
        return (Vector3.Dot (a, b) + 1) / 2;
    }
    public void SetRoom (Room _room)
    {
        parentRoom = _room;
        parentRoom.Closing += SetObjetive;
    }
    public void Die ()
    {
        parentRoom.RemoveEnemy ();
    }
}
