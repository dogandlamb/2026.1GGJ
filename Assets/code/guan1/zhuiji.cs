using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zhuiji : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    GameObject mainCharacter;
    void Start()
    {
        mainCharacter=GameObject.Find("nailong");
        speed=4;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void yidong()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity=speed*(mainCharacter.transform.position-gameObject.transform.position).normalized;
    }
    public void stop()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("peng");
        if (collision.gameObject==mainCharacter)
        {
            mainCharacter.GetComponent<mainCharacter>().isDamage(1);
            Debug.Log("damage");
        }
    }
}
