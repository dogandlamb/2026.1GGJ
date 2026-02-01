using System.Collections;
using System.Collections.Generic;
using System.Data;
// using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;



public class mainCharacter : MonoBehaviour
{
    int health;
    float speed;
    const int maskNumber=10;
    Rigidbody2D rb;
    SpriteRenderer sr;


    Vector3 cameraVelocity=Vector3.zero;
    
    //不同方向的移动贴图
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    int maskType;
    int currentMaskType;
    int[] maskAvailable=new int[maskNumber]{1,1,1,0,0,0,0,0,0,0};
    delegate void maskWear();
    delegate void maskAbility();
    maskWear[] maskWearArray=new maskWear[maskNumber];
    maskAbility[] maskAbilityArray=new maskAbility[maskNumber];
    //定义函数数组储存面具穿戴时触发的函数和面具穿戴后持续触发的函数




    
    // Start is called before the first frame update
    void Start()
    {
        health=3;
        speed=2;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        maskType=0;
        currentMaskType=0;
        // sightClear=1;
        maskinit();

    }

    // Update is called once per frame
    void Update()
    {
        move();
        maskTransform();
        maskAbilityArray[currentMaskType]?.Invoke();
        // Debug.Log(currentMaskType);
    }

    void move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity=new Vector3(0,speed,0);
            if(spriteUp != null) sr.sprite = spriteUp;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity=new Vector3(0,-speed,0);
            if(spriteDown != null) sr.sprite = spriteDown;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity=new Vector3(speed,0,0);
            if(spriteRight != null) sr.sprite = spriteRight;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity=new Vector3(-speed,0,0);
            if(spriteLeft != null) sr.sprite = spriteLeft;
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
    void getMask(int maskType)//获得的面具调用此函数，参数为面具的序号
    {
        maskAvailable[maskType]=1;
    }

    void maskinit()//初始化所有的面具的函数，无需操心
    {
        maskWearArray[0]=maskWear_buniaoni;
        maskWearArray[0]=maskWear_lingzhumowan;
        maskAbilityArray[0]=maskAbility_buniaoni;
        maskAbilityArray[1]=maskAbility_lingzhumowan;
    }
    static void maskWear_buniaoni(){}
    static void maskWear_lingzhumowan(){}
    static void maskAbility_buniaoni(){}
    
    static void maskAbility_lingzhumowan(){}
    public int getCurrentMaskType()
    {
        return currentMaskType;
    }
    public void isDamage(int damage)
    {
        health-=damage;
                Debug.Log(health);
    }

    public void visionBlur()
    {
        float cameraSmoothTime=0.3f;
        UnityEngine.Vector3 targetPosition=new UnityEngine.Vector3(10*Random.value-5,10*Random.value-5,-10);
        Camera.main.transform.position=UnityEngine.Vector3.SmoothDamp(
            Camera.main.transform.position,
            targetPosition,
            ref cameraVelocity,
            cameraSmoothTime
        );
        if(Camera.main.transform.position.x>10 || Camera.main.transform.position.x < -10 || Camera.main.transform.position.y>10 || Camera.main.transform.position.y < -10)
        {
            cameraReset();
        }
    }

    public void cameraReset()
    {
        Camera.main.transform.position=new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y,-10);
    }
}
