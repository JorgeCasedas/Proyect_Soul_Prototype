using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Singleton
    public static MenuManager Instance;
    private void Awake ()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy (this);
    }
    #endregion

    public GameObject pausePanel;
    public GameObject deathPanel;

    private void Start ()
    {
        pausePanel.SetActive (false);
        deathPanel.SetActive (false);
    }
    private void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape) && !GameManager.Instance.isCharacterDead)
        {
            pausePanel.SetActive (true);
        }
    }
    public void EnableDeathPanel ()
    {
        deathPanel.SetActive (true);
    }
}
