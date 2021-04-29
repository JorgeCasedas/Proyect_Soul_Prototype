using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimEvents : MonoBehaviour
{

    SoldierController controller;
    public GameObject weaponHitBox;
    public AudioSource stepAud;
    public AudioClip step;
    public AudioSource spearAud;
    public AudioClip slash;
    private void Awake()
    {
        controller = gameObject.transform.parent.gameObject.GetComponent<SoldierController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        weaponHitBox.SetActive (false);
    }

    public void StartAttack() //Called from AnimEvents
    {
        weaponHitBox.SetActive(true);
    }
    public void EndAttackDamage() //Called from AnimEvents
    {
        weaponHitBox.SetActive(false);
    }
    public void EndAttack() //Called from AnimEvents
    {
        controller.EndAttack();
    }
    public void EndSkill () //Called from AnimEvents
    {
        controller.EndSkill ();
    }
    public void StartMovementSkill ()
    {
        controller.MovSkill ();
    }
    public void EndMovementSkill ()
    {
        controller.EndMovementSkill ();
    }
    public void PlayStep ()
    {
        stepAud.clip = step;
        stepAud.Play ();
    }
    public void PlaySlash ()
    {
        spearAud.clip = slash;
        spearAud.Play ();
    }
}
