using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float step;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = target.position - transform.position; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target.parent != null)
            transform.position = Vector3.Lerp(transform.position, target.parent.position - offset, step * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(transform.position, target.position - offset, step * Time.deltaTime);
    }
}
