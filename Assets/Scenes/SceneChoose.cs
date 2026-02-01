using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChoose : MonoBehaviour
{
    public void ChangeToScene1()
    {
        SceneManager.LoadScene("InnerScene1");
    }

    public void ChangeToScene2()
    {
        SceneManager.LoadScene("InnerScene2");
    }

    public void ChangeToScene3()
    {
        SceneManager.LoadScene("InnerScene3");
    }

    public void ChangeToStart()
    {
        SceneManager.LoadScene("StartScene");
    }
}
