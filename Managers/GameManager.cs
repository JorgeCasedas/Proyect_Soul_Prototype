using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    LevelGeneration levelGeneration;
    public GameObject character;
    public bool isCharacterDead;
    private void Awake ()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy (this);
    }
    // Start is called before the first frame update
    void Start()
    {
        levelGeneration = LevelGeneration.Instance;
        isCharacterDead = false;
    }

    public void Lose ()
    {
        isCharacterDead = true;
        MenuManager.Instance.EnableDeathPanel ();
    }
}
