using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollAI : BaseAI
{
    BaseControl controller;

    public float attackDistance;
    float dashTimer;

    

    private void Awake()
    {
        controller = GetComponent<BaseControl>();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start ();
        controller.BecomeAlive();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller && objetive)
        {
            controller.SetPointToLookAt(objetive.transform.position);
            Vector3 dir = Vector3.zero;
            float distanceToObjective = Vector3.Distance(objetive.transform.position, transform.position);
            if (distanceToObjective < attackDistance)
            {
                controller.ForceToLookAt(objetive.transform.position);
                controller.Attack();
            }
            else
            {
                dashTimer += Time.deltaTime;
                if (dashTimer > 3)
                {
                    dashTimer = 0;
                    controller.MovementSkill(true);
                }
                dir = objetive.transform.position - transform.position;
            }
            controller.SetDirection(EvaluateDirection(dir));
        }          
    }
    
}
