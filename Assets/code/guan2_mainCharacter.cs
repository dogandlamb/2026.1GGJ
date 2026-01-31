using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;



public class guan2_mainCharacter : MonoBehaviour
{
    public int health; // 改为public以便UI访问
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
    public int[] maskAvailable=new int[maskNumber]{1,1,1,0,0,0,0,0,0,0}; // 改为public
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
        maskType=3; // 初始默认为3，以防从0开始
        currentMaskType=3; // 初始默认为3
        // 确保一开始就有3号面具(为了测试方便，或者你可以根据逻辑通过 getMask(3) 获得)
        maskAvailable[3] = 1;
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
        // 这一段是旧的左键确认逻辑，已删除
        /*
        if (Input.GetMouseButtonDown(0))
        {
            currentMaskType=maskType;
            maskWearArray[maskType]?.Invoke();
        }
        */

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 限定只在 3, 4, 5, 6 之间循环
            // 如果还没拿到对应的面具，就可能跳过
            // 如果刚开始是0，按第一下Q应该变成3 (如果3已解锁)
            
            // 简单循环查找：从当前 maskType + 1 开始找，直到找到一个 maskAvailable==1 且 ID >= 3 且 ID <= 6 的
            // 这只是针对这一关的特殊逻辑

            int checkId = maskType;
            for (int i = 0; i < maskNumber; i++) 
            {
                checkId++; // 检查下一个
                if (checkId > 6) checkId = 3; // 超过6回到3

                // 如果 maskAvailable 数组里说我们有这个面具
                if (maskAvailable[checkId] == 1)
                {
                    maskType = checkId;
                    break;
                }
            }
        }

        // 实时更新 currentMaskType，使其始终等于选中的 maskType
        if (currentMaskType != maskType)
        {
            currentMaskType = maskType;
            maskWearArray[maskType]?.Invoke(); // 触发戴面具效果
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
