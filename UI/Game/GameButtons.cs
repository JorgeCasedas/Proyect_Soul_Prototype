using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtons : MonoBehaviour
{
    public void Resume ()
    {
        MenuManager.Instance.pausePanel.SetActive (false);
    }
    public void Restart ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
    public void GoBackToMenu ()
    {
        SceneManager.LoadScene (0);
    }
}
