using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : BaseControl
{
    bool isDefending;
    [SerializeField]
    float knockBackForce;
    [SerializeField]
    float rollSpeed;
    [SerializeField]
    float rollCooldown;
    float internalRollCooldown;
    [SerializeField]
    AudioSource rollAud;
    [SerializeField]
    AudioSource shieldAud;

    // Start is called before the first frame update
    void Start()
    {
        internalRollCooldown = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!alive)
            return;
        if (!doingAction) //Movement
        {
            velocity.y = 0;
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
            lastLookAt.y = transform.position.y;
            transform.LookAt(lastLookAt);
            rb.velocity = velocity;
        }
        if (internalRollCooldown > 0)
        {
            internalRollCooldown -= Time.deltaTime;
            if(isPossesed)
                UIManager.Instance.FillMovementSkill (internalRollCooldown, rollCooldown);
        }
    }
    public bool IsDefending ()
    {
        return isDefending;
    }
    public override void BecomeAlive()
    {
        base.BecomeAlive();
        anim.SetTrigger("live");
        alive = true;
        doingAction = false;
        if (isPossesed)
            UIManager.Instance.FillMovementSkill (internalRollCooldown, rollCooldown);
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
        //base.Attack();
        SetDirection (Vector3.zero);
        rb.velocity = velocity;
        anim.SetTrigger("attack");
        doingAction = true;
        lastLookAt.y = transform.position.y;
        transform.LookAt(lastLookAt);
    }

    public override void EndAttack()
    {
        //base.EndAttack();
        doingAction = false;
    }

    public override void MovementSkill(bool IACall)
    {
        //base.MovementSkill(IACall);  
        if (internalRollCooldown<=0)
            anim.SetTrigger ("movSkill");
    }
    public void MovSkill ()
    {
        rollAud.Play ();
        doingAction = true;
        rb.velocity = velocity * rollSpeed;
        internalRollCooldown = rollCooldown;
    }
    public override void EndMovementSkill ()
    {
        //base.EndMovementSkill ();
        rb.velocity = Vector3.zero;
        doingAction = false;
    }

    public override void Skill ()
    {
        if (doingAction)
            return;
        base.Skill ();
        SetDirection (Vector3.zero);
        rb.velocity = velocity;
        anim.SetTrigger ("skill");
        doingAction = true;
        isDefending = true;
    }
    public override void EndSkill ()
    {
        //base.EndSkill ();
        doingAction = false;
        isDefending = false;
    }
    public override void GetHit (GameObject enemy, Vector3 weaponHitBox)
    { 
        if (isDefending)
            KnockBack (enemy.transform.position);
        else
            base.GetHit (enemy, weaponHitBox);
    }
    void KnockBack (Vector3 enemyPos)
    {
        shieldAud.Play ();
        rb.AddForce ((transform.position - enemyPos).normalized * knockBackForce, ForceMode.Impulse);
    }
}
