using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public GameObject hitParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "stats")
        {
            other.GetComponent<BaseStats> ().GetHit (gameObject, transform.position, damage);
            Instantiate(hitParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
       
    }
}
