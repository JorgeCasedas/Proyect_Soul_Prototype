using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableItem : MonoBehaviour, ISpawnableObject
{
    public Vector2 heightMinMax;
    public Vector2 timeToFallMinMax;
    string poolTag;

    public float movSpeed;
    float timeToGoToPlayer;
    bool goToPlayer;

    private void Update ()
    {
        if (goToPlayer)
            timeToGoToPlayer -= Time.deltaTime;
        if (timeToGoToPlayer < 0)
            transform.position += (GameManager.Instance.character.transform.position - transform.position).normalized * movSpeed;
    }
    // Update is called once per frame
    void DropAnim ()
    {  
        Vector3 targetPos = new Vector3 (Random.Range (-1, 1f), 0, Random.Range (-1, 1f));
        targetPos = transform.position + ( targetPos.normalized * Random.Range (1, 3f));
        StartCoroutine(MoveToPos (targetPos));
    }
    IEnumerator MoveToPos (Vector3 targetPos)
    {
        float lerp = 0;
        Vector3 pos = transform.position;
        float height = Random.Range (heightMinMax.x, heightMinMax.y);
        float timeToFall = Random.Range (timeToFallMinMax.x, timeToFallMinMax.y);

        while (lerp < 1)
        {
            lerp += Time.deltaTime / timeToFall;
            Vector3 nextPos = Vector3.Lerp (pos, targetPos, lerp);
            nextPos.y += Mathf.Sin (lerp * Mathf.PI) * height;
            transform.position = nextPos;
            yield return null;
        }
        goToPlayer = true;
    }

    public void OnSpawn (string _poolTag)
    {
        goToPlayer = false;
        timeToGoToPlayer = Random.Range (0.5f, 1);
        poolTag = _poolTag;
        DropAnim ();
    }
    public void OnPickUp ()
    {
        gameObject.SetActive (false);
        PoolManager.Instance.AddToPool (poolTag, gameObject);
    }
    private void OnTriggerEnter (Collider other)
    {
        OnPickUp ();
    }
}
