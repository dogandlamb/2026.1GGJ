using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;



public class mainCharacter : MonoBehaviour
{
    int health;
    float speed;
    const int maskNumber=10;
    // GameObject mainCharacter=GameObject.Find("mainCharacter");
    Rigidbody2D rb;

    int maskType;
    int currentMaskType;
    int[] maskAvailable=new int[maskNumber]{1,1,1,0,0,0,0,0,0,0};
    delegate void maskWear();
    delegate void maskAbility();
    maskWear[] maskWearArray=new maskWear[maskNumber];
    maskAbility[] maskAbiliyArray=new maskAbility[maskNumber];

    


    
    // Start is called before the first frame update
    void Start()
    {
        health=3;
        speed=2;
        rb = GetComponent<Rigidbody2D>();
        maskType=0;
        currentMaskType=0;

    }

    // Update is called once per frame
    void Update()
    {
        move();
        maskTransform();
        maskAbiliyArray[currentMaskType]?.Invoke();
        Debug.Log(currentMaskType);
    }

    void move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity=new Vector3(0,speed,0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity=new Vector3(0,-speed,0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity=new Vector3(speed,0,0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity=new Vector3(-speed,0,0);
        }
        else
        {
            rb.velocity=new Vector3(0,0,0);
        }

    }
    void maskTransform()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentMaskType=maskType;
            maskWearArray[maskType]?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for(int i = 1; i <= maskNumber; i++)
            {
                if (i+maskType >= maskNumber)
                {
                    if (maskAvailable[i+maskType-10] == 1)
                    {
                        maskType=i+maskType-10;
                        break;
                    }
                }
                else
                {
                    if (maskAvailable[i+maskType] == 1)
                    {
                        maskType=i+maskType;
                        break;
                    }
                }
            }
        }
    }
    void getMask(int maskType)
    {
        maskAvailable[maskType]=1;
    }
    static void Mask1(){}
    static void Mask0(){}
}
