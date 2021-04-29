using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitchAnimEvents : MonoBehaviour
{
    LitchController controller;

    public Transform projectileOrigin;
    public GameObject projectilePrefab;
    Quaternion projectileSpawningRot;

    public AudioSource aud;
    public AudioClip step;

    public AudioSource shotAud;
    private void Awake()
    {
        controller = gameObject.transform.parent.gameObject.GetComponent<LitchController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootProjectile()
    {
        
        projectileSpawningRot = Quaternion.Euler(new Vector3(0, projectileOrigin.eulerAngles.y-90, 0));
        Instantiate(projectilePrefab, projectileOrigin.position, transform.rotation);
        shotAud.Play ();
    }

    public void EndAttack()
    {
        controller.EndAttack();
    }
    public void PlayStep ()
    {
        aud.clip = step;
        aud.Play ();
    }
}
