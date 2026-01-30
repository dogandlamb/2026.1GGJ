using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            switchToScene("VictoryScene");
            Debug.Log("°´¼ü: " + Input.inputString);
        }
    }

    void switchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
