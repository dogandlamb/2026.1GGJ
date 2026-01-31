using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroduceCode : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeToStart()
    {
        SceneManager.LoadScene("StartScene");
    }
}
