using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollController : BaseControl
{


    bool dashing;
    bool waitingDash;
    public float dashDuration;
    public float dashCooldown;
    float internalDashCooldown;
    Vector3 lastVelocity;

    
  
    // Start is called before the first frame update
    void Start()
    {
        dashing = false;
        internalDashCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;
        if (!doingAction) //Movement
        {
            if (dashing && !waitingDash)
            {
                velocity = lastVelocity;
            }
            lastVelocity = velocity;
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
        }
        else
        {
            velocity = Vector3.zero;
        }
        if(dashing && waitingDash)
        {
            internalDashCooldown -= Time.deltaTime;
            if (internalDashCooldown <= 0)
            {
                waitingDash = false;
                dashing = false;
            }
            if (isPossesed)
                UIManager.Instance.FillMovementSkill (internalDashCooldown, dashCooldown);
        }
        rb.velocity = velocity;
    }

    public void Dash()
    {
        if (!dashing)
        {
            dashing = true;
            waitingDash = false;
            internalDashCooldown = dashCooldown;
            if (isPossesed)
                UIManager.Instance.FillMovementSkill (internalDashCooldown, dashCooldown);
            StartCoroutine (EndDash());
        }
    }
    IEnumerator EndDash()
    {
        float actualSpeed = speed;
        speed *= 10;
        yield return new WaitForSeconds(dashDuration);
        waitingDash = true;
        speed = actualSpeed;
    }

    public override void BecomeAlive()
    {
        base.BecomeAlive();
        anim.SetTrigger("live");
        alive = true;
        doingAction = false;
        if (isPossesed)
            UIManager.Instance.FillMovementSkill (internalDashCooldown, dashCooldown);
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
        lastLookAt.y = transform.position.y;
        transform.LookAt(lastLookAt);
    }

    public override void EndAttack()
    {
        base.EndAttack();
        doingAction = false;
    }

    public override void MovementSkill(bool IACall)
    {
        base.MovementSkill(IACall);
        Dash();
    }
}
