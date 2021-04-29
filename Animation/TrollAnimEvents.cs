using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollAnimEvents : MonoBehaviour
{
    TrollController controller;
    public GameObject weaponHitBox;
    public AudioSource stepAud;
    public AudioClip step;
    public AudioSource weaponAud;
    public AudioClip[] slashs;
    private void Awake()
    {
        controller = gameObject.transform.parent.gameObject.GetComponent<TrollController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        weaponHitBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void PlayStep ()
    {
        stepAud.clip = step;
        stepAud.Play ();
    }
    public void PlaySlash ()
    {
        weaponAud.clip = slashs[Random.Range(0,slashs.Length)];
        weaponAud.Play ();
    }
}
