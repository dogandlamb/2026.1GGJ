// using System.Collections;
// using System.Collections.Generic;
// using System.Diagnostics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class fangjiao : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject mainCharacter;
    void Start()
    {
        mainCharacter=GameObject.Find("nailong");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(mainCharacter.name);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("peng");
        if (collision.gameObject==mainCharacter && mainCharacter.GetComponent<mainCharacter>().getCurrentMaskType()!=0)
        {
            mainCharacter.GetComponent<mainCharacter>().isDamage(1);
            // Debug.Log("damage");
        }
    }
}
