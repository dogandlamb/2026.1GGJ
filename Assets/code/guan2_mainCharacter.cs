using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;



public class guan2_mainCharacter : MonoBehaviour
{
    int health;
    public float speed;
    public float jumpForce;
    const int maskNumber=10;
    // GameObject mainCharacter=GameObject.Find("mainCharacter");
    Rigidbody2D rb;
    SpriteRenderer sr;
    
    //不同方向的移动贴图
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    public int maskType;
    public int currentMaskType;
    int[] maskAvailable=new int[maskNumber]{1,1,1,0,0,0,0,0,0,0};
    delegate void maskWear();
    delegate void maskAbility();
    maskWear[] maskWearArray=new maskWear[maskNumber];
    maskAbility[] maskAbiliyArray=new maskAbility[maskNumber];

    


    public int GetCurrentMaskType()
    {
        return currentMaskType;
    }

    // Start is called before the first frame update
    void Start()
    {
        health=3;
        speed=5;
        jumpForce=8;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1; // 启用重力
        sr = GetComponent<SpriteRenderer>();
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
        float moveX = 0;
        if (Input.GetKey(KeyCode.D))
        {
            moveX = speed;
            if(spriteRight != null) sr.sprite = spriteRight;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveX = -speed;
            if(spriteLeft != null) sr.sprite = spriteLeft;
        }
        
        // 应用水平速度，保留垂直速度（重力/跳跃）
        rb.velocity = new Vector2(moveX, rb.velocity.y);

        // 上下跳跃 (W键) - 简单的落地检测
        // 注意：这需要Rigidbody2D的Gravity Scale不为0
        if (Input.GetKeyDown(KeyCode.W) && Mathf.Abs(rb.velocity.y) < 0.05f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if(spriteUp != null) sr.sprite = spriteUp;
        }

        // 下键可用于下蹲或切换图片
        if (Input.GetKey(KeyCode.S))
        {
            if(spriteDown != null) sr.sprite = spriteDown;
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
    public void getMask(int maskType)
    {
        maskAvailable[maskType]=1;
        Debug.Log("Unlocked mask: " + maskType);
    }

    // 处理触发器碰撞
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Penguin"))//企鹅被设置为IsTrigger
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage()
    {
        health--;
        Debug.Log("受到伤害！当前生命值: " + health);
        if (health <= 0)
        {
            health = 0;
            Debug.Log("游戏结束 (Player Died)");
            // 这里可以添加重新加载场景或死亡动画的逻辑
        }
    }

    public bool HasMask(int maskId)
    {
        if(maskId < 0 || maskId >= maskAvailable.Length) return false;
        return maskAvailable[maskId] == 1;
    }
    
    static void Mask1(){}
    static void Mask0(){}
}
