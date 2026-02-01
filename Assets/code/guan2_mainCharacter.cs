using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // 引入 SceneManagement



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

    [Header("不同面具的下蹲(按S)贴图")]
    public Sprite spriteDownNoMask; // 无面具/默认
    public Sprite spriteDownMask3;  // 面具3
    public Sprite spriteDownMask4;  // 面具4
    public Sprite spriteDownMask5;  // 面具5
    public Sprite spriteDownMask6;  // 面具6

    [Header("音效")]
    public AudioSource audioSource;
    public AudioClip sfxMove;       // 移动/走路
    public AudioClip sfxJump;       // 跳跃
    public AudioClip sfxMaskChange; // 切换面具
    public AudioClip sfxDamage;     // 受伤

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
        // 获取 AudioSource，如果没有则尝试获取物体上的
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        maskType = 0; // 初始设为0（默认无特殊面具），避免一开始就变成3
        currentMaskType = 0; 
        
        // 注意：除了 maskAvailable[0,1,2] 可能作为基础面具外，其他都应该为 0
        // 代码开头定义的 int[] maskAvailable=new int[maskNumber]{1,1,1,0...} 已经默认了3号是0
        // 之前我加了一行 maskAvailable[3] = 1，现在删掉它
    }

    // Update is called once per frame
    void Update()
    {
        move();
        maskTransform();
        if (currentMaskType >= 0 && currentMaskType < maskNumber)
        {
            maskAbiliyArray[currentMaskType]?.Invoke();
        }
        Debug.Log(currentMaskType);
    }

    void move()
    {
        float moveX = 0;
        bool isMoving = false;

        if (Input.GetKey(KeyCode.D))
        {
            moveX = speed;
            isMoving = true;
            if(spriteRight != null) sr.sprite = spriteRight;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveX = -speed;
            isMoving = true;
            if(spriteLeft != null) sr.sprite = spriteLeft;
        }
        
        // 处理移动音效 (循环播放)
        if (isMoving && Mathf.Abs(rb.velocity.y) < 0.1f) // 在地面上移动才响
        {
            if (audioSource != null && sfxMove != null && (!audioSource.isPlaying || audioSource.clip != sfxMove))
            {
                audioSource.clip = sfxMove;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            // 如果停止移动或者在这个AudioSource上播放的是移动音效，则停止
            if (audioSource != null && audioSource.isPlaying && audioSource.clip == sfxMove)
            {
                audioSource.Stop();
                audioSource.clip = null; 
            }
        }

        // 应用水平速度，保留垂直速度（重力/跳跃）
        rb.velocity = new Vector2(moveX, rb.velocity.y);

        // 上下跳跃 (W键) - 简单的落地检测
        // 注意：这需要Rigidbody2D的Gravity Scale不为0
        if (Input.GetKeyDown(KeyCode.W) && Mathf.Abs(rb.velocity.y) < 0.05f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if(spriteUp != null) sr.sprite = spriteUp;
            
            // 播放跳跃音效
            if (audioSource != null && sfxJump != null) 
                audioSource.PlayOneShot(sfxJump);
        }

        // 下键可用于下蹲或切换图片
        if (Input.GetKey(KeyCode.S))
        {
            Sprite targetDown = spriteDownNoMask; // 默认使用无面具图
            
            if (currentMaskType == 3) targetDown = spriteDownMask3;
            else if (currentMaskType == 4) targetDown = spriteDownMask4;
            else if (currentMaskType == 5) targetDown = spriteDownMask5;
            else if (currentMaskType == 6) targetDown = spriteDownMask6;

            if (targetDown != null) sr.sprite = targetDown;
            else if(spriteDown != null) sr.sprite = spriteDown;
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
            
            // 播放切换面具音效
            if (audioSource != null && sfxMaskChange != null)
                audioSource.PlayOneShot(sfxMaskChange);
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
        
        // 播放受伤音效
        if (audioSource != null && sfxDamage != null)
            audioSource.PlayOneShot(sfxDamage);

        Debug.Log("受到伤害！当前生命值: " + health);
        if (health <= 0)
        {
            health = 0;
            Debug.Log("游戏结束 (Player Died)");
            // 跳转到复活场景
            SceneManager.LoadScene("ReliveScene"); 
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
