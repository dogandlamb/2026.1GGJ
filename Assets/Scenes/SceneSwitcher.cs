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
        Debug.Log("sceneName");
        if (Input.GetKeyDown(KeyCode.V))
        {
            switchToScene();
        }
        if(Input.anyKeyDown)
        {
            switchToScene();
            Debug.Log("°´¼ü: " + Input.inputString);
        }
    }

    void switchToScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("sceneName");
    }
}
