using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseControl : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody rb;
    protected Camera mainCamera;

    public float speed;

    protected Vector3 velocity;
    protected Vector3 pointToLookAt;

    protected bool doingAction;
    protected Vector3 lastLookAt;

    protected bool alive;
    protected bool canBePossesed;
    public bool isPossesed;

    public float hitDamage;

    public delegate void BaseEvent ();
    public event BaseEvent OnDeath;
    public event BaseEvent OnAIDeath;

    public GameObject hitParticles;
    
    private void Awake ()
    {
        anim = transform.GetChild (0).GetComponent<Animator> ();
        rb = GetComponent<Rigidbody> ();
        mainCamera = Camera.main;
    }
    public bool CanBePossesed ()
    {
        return canBePossesed;
    }
    public void SetCanBePossesed (bool can)
    {
        canBePossesed = can;
    }
    virtual public void BecomeAlive()
    {
        transform.Find("Stats").GetComponent<BaseStats> ().SetHp ();   
    }
    virtual public void GetHit (GameObject enemy, Vector3 weaponHitBox)
    {
        Instantiate (hitParticles, weaponHitBox, transform.rotation);
    }

    virtual public void Die()
    {
        OnDeath?.Invoke ();
        BaseAI ai = GetComponent<BaseAI> ();
        if (ai)
        {
            ai.Die ();
            Destroy (ai);
            SetCanBePossesed (true);
            OnAIDeath?.Invoke ();
        }
        else
        {
            Dissapear ();
        }
    }
    public void Dissapear ()
    {
        canBePossesed = false;
        Destroy (GetComponent<Rigidbody> ());
        Destroy (GetComponent<Collider> ());
        SkinnedMeshRenderer mesh = GetComponentInChildren<SkinnedMeshRenderer> ();
        mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        StartCoroutine (DissapearSlowly (mesh));
    }
    IEnumerator DissapearSlowly (SkinnedMeshRenderer mesh)
    {
        float i= mesh.material.GetFloat ("_Amount");
        while (i < 1)
        {
            i += Time.deltaTime/3;
            mesh.material.SetFloat ("_Amount", i);
            yield return null;
        }
        Destroy (gameObject);
    }

    virtual public void Attack()
    {

    }

    virtual public void EndAttack()
    {

    }

    virtual public void MovementSkill(bool IACall)
    {

    }
    virtual public void EndMovementSkill ()
    {

    }

    virtual public void Skill ()
    {

    }
    virtual public void EndSkill ()
    {

    }

    public void SetDirection(Vector3 dir)
    {
        velocity = dir;
    }

    

    public void SetPointToLookAt(Vector3 newPoint)
    {
        pointToLookAt = newPoint;
    } 
    public void ForceToLookAt(Vector3 newPoint)
    {
        lastLookAt = newPoint;
    }

    public bool IsAlive()
    {
        return alive;
    }
}
