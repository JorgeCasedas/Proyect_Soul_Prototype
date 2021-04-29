using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitchController : BaseControl
{
    public float teleportDistance;
    public float teleportCooldown;
    float internalTpCooldown;
    public GameObject tpParticles;

    // Start is called before the first frame update
    void Start()
    {
        internalTpCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;
        if (!doingAction)
        {
            velocity = velocity.normalized * speed;
            if (velocity.magnitude > 0)
            {
                anim.SetBool("walk", true);
            }
            else
            {
                anim.SetBool("walk", false);
            }
            lastLookAt = pointToLookAt;
        }
        else
        {
            velocity = Vector3.zero;
        }
        internalTpCooldown -= Time.deltaTime;
        if (isPossesed)
            UIManager.Instance.FillMovementSkill (internalTpCooldown, teleportCooldown);
        rb.velocity = velocity;
        lastLookAt.y = transform.position.y;
        transform.LookAt(lastLookAt);
    }

    void Teleport(bool IACall)
    {
        if (internalTpCooldown > 0)
            return;
        Instantiate(tpParticles, transform.position, transform.rotation);
        RaycastHit hit;
        int layerMask = 1 << 7;
        if (IACall)
        {
            Vector3 dir = new Vector3 (Random.Range (0f, 1f), 0, Random.Range (0f, 1f)).normalized;
            if (Physics.Raycast (transform.position, dir, out hit, teleportDistance, layerMask))
            {      
                transform.position = hit.point - dir * 0.2f;
            }
            else
            {
                transform.position += dir * teleportDistance;
            }
        }
            
        else
        {
            
            if (Physics.Raycast (transform.position, velocity, out hit, teleportDistance, layerMask))
            {
                Vector3 dir = (hit.point - transform.position).normalized;
                transform.position = hit.point - dir*0.2f;
                Debug.Log (hit.collider.name);
            }
            else
            {
                transform.position += velocity.normalized * teleportDistance;
            }
        }
            
        internalTpCooldown = teleportCooldown;
    }

    public override void BecomeAlive()
    {
        base.BecomeAlive();
        anim.SetTrigger("live");
        alive = true;
        doingAction = false;
        if (isPossesed)
            UIManager.Instance.FillMovementSkill (internalTpCooldown, teleportCooldown);
    }

    public override void Die()
    {
        base.Die();
        anim.SetTrigger("die");
        SetDirection(Vector3.zero);
        alive = false;         
    }

    public override void Attack()
    {
        if (doingAction)
            return;
        base.Attack();
        anim.SetTrigger("attack");
        doingAction = true;
    }

    public override void EndAttack()
    {
        base.EndAttack();
        doingAction = false;
    }

    public override void MovementSkill(bool IACall)
    {
        base.MovementSkill(IACall);
        Teleport(IACall);
    }
}
