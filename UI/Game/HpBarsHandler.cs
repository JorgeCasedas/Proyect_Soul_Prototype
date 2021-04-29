using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarsHandler : MonoBehaviour
{
    Animator anim;

    public GameObject soulHp;
    public GameObject bodyHp;

    public GameObject soulHpFill;
    public GameObject bodyHpFill;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator> ();
    }

    public void ChangeToSoulHp ()
    {
        anim.SetTrigger ("ToSoul");
    }
    public void ChangeToBodyHp ()
    {
        anim.SetTrigger ("ToBody");
    }

    public void SetBodyHp (float hp, float maxHp)
    {
        bodyHpFill.GetComponent<Image> ().fillAmount = hp / maxHp;
    }
    public void SetSoulHp (float hp, float maxHp)
    {
        soulHpFill.GetComponent<Image> ().fillAmount = hp / maxHp;
    }

    public void SetSoulHpFirst () //Called from anim events
    {
        soulHp.transform.SetAsLastSibling ();
    }
    public void SetBodyHpFirst () //Called from anim events
    {
        bodyHp.transform.SetAsLastSibling ();
    }
}
