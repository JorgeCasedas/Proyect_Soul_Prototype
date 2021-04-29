using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour //MainController
{
    public float speed;
    public float cooldownAfterPossesing;
    float possesingCooldown;
    bool possesing;
    GameObject objectToPosses;
    Rigidbody rb;
    Camera mainCamera;

    public ParticleSystem lightning;
    public float attackCooldown;
    float internalAttackCooldown;

    BaseControl controller;
    GameObject stats;
    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponentInChildren<BaseStats> ().gameObject;
        possesingCooldown = cooldownAfterPossesing;
        internalAttackCooldown = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (possesingCooldown > 0)
        {
            possesingCooldown -= Time.deltaTime;
        }
        Vector3 velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            velocity += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity += -Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += -Vector3.right;
        }
        velocity = velocity.normalized * speed;

        

        if (Input.GetKeyDown(KeyCode.E))
        {
            Posses();
        }

        if (!possesing)
        {
            if (Input.GetMouseButtonDown (0))
            {
                Attack ();
            }
            internalAttackCooldown -= Time.deltaTime;
            rb.velocity = velocity;
        }
        else   //Possesed movement
        {
            rb.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
            controller.SetDirection(velocity);
            if (Input.GetMouseButtonDown(0))
            {
                controller.Attack();
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                controller.MovementSkill(false);
            }
            if (Input.GetMouseButtonDown (1))
            {
                controller.Skill ();
            }
        }
    }

    private void FixedUpdate()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);
            if (!possesing)
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            else
                controller.SetPointToLookAt(pointToLook);
        }
    }

    void Attack ()
    {
        if(internalAttackCooldown<0)
        {
            internalAttackCooldown = attackCooldown;
            lightning.Play ();
        }
    }
    void Posses()
    {
        if (!objectToPosses || possesingCooldown>0)
            return;
        if (possesing)
        {
            possesing = false;
        }
        else 
        {
            if (objectToPosses.GetComponent<BaseControl>().IsAlive() || !objectToPosses.GetComponent<BaseControl> ().CanBePossesed())
                return;
            if (objectToPosses)
            {
                possesing = true;
            }
        }
        TransferSoul();
    }

    void TransferSoul()
    {
        StopAllCoroutines();
        if (possesing)  //GET IN
        {
            if (objectToPosses)
                objectToPosses.GetComponent<AnimateFresnel> ().ChangeColor (Color.black);
            stats.SetActive (false);
            transform.parent = objectToPosses.transform;
            StartCoroutine(MoveToParentCenter());
            controller = objectToPosses.GetComponent<BaseControl>();
            UIManager.Instance.ChangeToBody ();
            controller.OnDeath += GetOutFromBody;
            controller.isPossesed = true;

            controller.BecomeAlive ();
            possesingCooldown = cooldownAfterPossesing;
        }
        else   //GET OUT
        {
            controller.Die ();
            GetOutFromBody ();
        }
        
    }

    void GetOutFromBody ()
    {
        controller.OnDeath -= GetOutFromBody;
        controller.isPossesed = false;
        transform.parent = null;
        possesing = false;
        UIManager.Instance.ChangeToSoul ();
        stats.SetActive (true);
    }

    IEnumerator MoveToParentCenter()
    {
        Vector3 initialLocalPos = transform.localPosition;
        Vector3 vZero = new Vector3(0, initialLocalPos.y, 0);
        float t = 0;
        while (transform.localPosition.x != vZero.x)
        {
            t += Time.deltaTime;
            transform.localPosition =  Vector3.Lerp(initialLocalPos, vZero, t);
            yield return null;
        }  
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "ownable" && !possesing)
        {
            if (objectToPosses)
                objectToPosses.GetComponent<AnimateFresnel> ().ChangeColor (Color.black);
            objectToPosses = other.gameObject;
            BaseControl baseC = objectToPosses.GetComponent<BaseControl> ();
            if (!baseC.IsAlive () && baseC.CanBePossesed())
                objectToPosses.GetComponent<AnimateFresnel> ().ChangeColor (Color.white);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ownable" && !possesing)
        {
            if (objectToPosses)
                objectToPosses.GetComponent<AnimateFresnel> ().ChangeColor (Color.black);
            objectToPosses = null;
        }
    }

    
}
