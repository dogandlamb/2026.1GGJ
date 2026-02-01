using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canxiang : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject mainCharacter;
    GameObject zhuijiguai;
    void Start()
    {
        mainCharacter=GameObject.Find("nailong");
        zhuijiguai=GameObject.Find("zhuijiguai");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(mainCharacter.name);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log(mainCharacter.name);
        if (collision.gameObject == mainCharacter)
        {
            Debug.Log(1);
            blurMCVision();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log(mainCharacter.name);
        mainCharacter.GetComponent<mainCharacter>().cameraReset();
        zhuijiguai.GetComponent<zhuiji>().stop();
    }

    void blurMCVision()
    {
        if (mainCharacter.GetComponent<mainCharacter>().getCurrentMaskType() != 1)
        {
            mainCharacter.GetComponent<mainCharacter>().visionBlur();
            zhuijiguai.GetComponent<zhuiji>().yidong();
        }
        else
        {
            mainCharacter.GetComponent<mainCharacter>().cameraReset();
            zhuijiguai.GetComponent<zhuiji>().stop();
        }
    }
}
