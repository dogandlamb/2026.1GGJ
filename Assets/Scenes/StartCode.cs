using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCode : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Choose");
    }
    public void GameQuit()
    {
        Application.Quit();
    }
    public void GameCredit()
    {
        SceneManager.LoadScene("Introduce");
    }
    public void GameCredit2Start()
    {
        SceneManager.LoadScene("StartScene");
    }
}
