using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    GameObject rootParent;

    protected BaseControl controller;
    public float maxHealth;
    public float health;

    SpawnFromPool spawnFromPool;
   
    private void Awake()
    {
        controller = transform.parent.gameObject.GetComponent<BaseControl>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rootParent = controller.gameObject;
        spawnFromPool = GetComponent<SpawnFromPool> ();
        controller.SetCanBePossesed (true);
        health = maxHealth;
        controller.OnAIDeath += ReStartHealth;
        controller.OnAIDeath += DropOnDeath;
    }

    void DropOnDeath ()
    {
        spawnFromPool.SpawnObjects ();
    }
    void ReStartHealth ()
    {
        health = maxHealth;
    }
    public virtual void SetHp ()
    {
        if (controller.isPossesed)
            UIManager.Instance.SetBodyHp (health, maxHealth);
    }
    public virtual void AddHealth(float addition)
    { 
        if (health <= 0 || !controller.IsAlive())
            return;
        health += addition;
        SetHp ();
        if (health <= 0)
        {
            controller.SetCanBePossesed (false);
            controller.Die (); 
        }
    }
    public virtual void GetHit(GameObject enemy, Vector3 weaponHitBox, float damage)
    {
        if (!controller.IsAlive ())
            return;
        controller.GetHit (enemy, weaponHitBox);
        AddHealth (-damage);
        //Debug.Log (enemy.name + " -> " + transform.root.gameObject.name);
    }

    private void OnTriggerEnter (Collider other)
    {
        if(!rootParent)
            rootParent = transform.root.gameObject;

        if (other.transform.root.gameObject.GetInstanceID() != rootParent.GetInstanceID() && other.tag == "weaponHitBox")
        {
            BaseControl baseC = other.transform.GetComponentInParent<BaseControl> ();
            GetHit (baseC.gameObject, other.transform.position, baseC.hitDamage);
        }
    }
}
