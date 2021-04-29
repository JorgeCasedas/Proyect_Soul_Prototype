using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitchAI : BaseAI
{

    BaseControl controller;

    Camera mainCamera;

    float timeToAttack;
    public float safeDistance;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        controller = GetComponent<BaseControl>();
    }
    // Start is called before the first frame update
    protected override void Start ()
    {
        base.Start ();
        timeToAttack = Random.Range (0.2f, 3f);
        controller.BecomeAlive ();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller && objetive)
        {
            controller.SetPointToLookAt(objetive.transform.position);
            if (timeToAttack < 0)
            {
                controller.ForceToLookAt(objetive.transform.position);
                controller.Attack();
                timeToAttack = Random.Range(0.2f, 3f);
            }

            timeToAttack -= Time.deltaTime;

            float distanceToObjective = Vector3.Distance(objetive.transform.position, transform.position);
            if(distanceToObjective < 5)
            {
                controller.MovementSkill(true);
            }
            if ( distanceToObjective < safeDistance)
            {
                controller.SetDirection(EvaluateDirection( transform.position - objetive.transform.position ));
            }
            else if(distanceToObjective > safeDistance + 5)
            {
                controller.SetDirection (EvaluateDirection (objetive.transform.position - transform.position));
            }
        }
    }
}
