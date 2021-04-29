using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFresnel : MonoBehaviour
{
    public float initFresnelValue = 1;
    SkinnedMeshRenderer bodyMesh;
    // Start is called before the first frame update
    void Start()
    {
        bodyMesh = GetComponentInChildren<SkinnedMeshRenderer> ();
        bodyMesh.material.SetFloat ("_FresnelExponent", initFresnelValue);
    }

    public void ChangeColor (Color color)
    {
        StopAllCoroutines ();
        StartCoroutine (ChangeBodyFresnelColor (color));
    }
    IEnumerator ChangeBodyFresnelColor (Color color)
    {
        Color initColor = bodyMesh.material.GetColor ("_FresnelColor");
        float lerp = 0;
        while (lerp < 1)
        {
            Color setColor = Color.Lerp (initColor, color, lerp);
            bodyMesh.material.SetColor ("_FresnelColor", setColor);
            lerp += Time.deltaTime*10;
            yield return null;
        }
    }
}
