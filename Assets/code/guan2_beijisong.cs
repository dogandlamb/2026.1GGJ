using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class guan2_beijisong : MonoBehaviour
{
    [Header("设置")]
    public guan2_mainCharacter playerScript;
    public int qinShiHuangMaskID = 3; // 秦始皇面具ID
    
    [Header("UI")]
    public TMP_Text hintText; // 显示"按J骑"的文本
    public Transform seatPoint; // 北极熊背上的位置

    [Header("北极熊参数")]
    public float bearSpeed = 4f;

    private bool canInteract = false;
    private bool isRiding = false;
    private Rigidbody2D playerRb;
    
    private Vector3 initialBearScale; // 记录北极熊的初始缩放

    // 原始状态备份（让北极熊可以恢复控制权或仅作为载具移动）
    // 为了简化，骑乘时直接移动北极熊，且带着玩家移动

    void Start()
    {
        initialBearScale = transform.localScale; // 记录初始大小
        if (hintText != null) hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        // 骑乘逻辑
        if (isRiding)
        {
            HandleRidingMovement();
            
            // 按K下车
            if (Input.GetKeyDown(KeyCode.K))
            {
                Dismount();
            }
        }
        // 还没骑，但在范围内
        else if (canInteract)
        {
            // 按J上车
            if (Input.GetKeyDown(KeyCode.J))
            {
                TryMount();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            if (playerScript == null) 
                playerScript = other.GetComponent<guan2_mainCharacter>();
            
            // 如果没骑，显示提示
            if (!isRiding && hintText != null)
            {
                hintText.gameObject.SetActive(true);
                hintText.text = "按J骑，K下车";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 只有当没骑的时候才算离开了范围（骑的时候玩家是子物体，可能导致逻辑变化，这里简化处理）
            // 如果正在骑行，通常不会触发Exit，因为玩家跟着动
            if (!isRiding)
            {
                canInteract = false;
                if (hintText != null) hintText.gameObject.SetActive(false);
            }
        }
    }

    void TryMount()
    {
        if (playerScript == null) return;

        // 检查面具
        if (playerScript.GetCurrentMaskType() == qinShiHuangMaskID)
        {
            Mount();
        }
        else
        {
            if (hintText != null) hintText.text = "需要秦始皇面具！";
        }
    }

    void Mount()
    {
        isRiding = true;
        canInteract = false; // 骑上去了就不再显示交互提示

        // 隐藏提示
        if (hintText != null) hintText.gameObject.SetActive(false);

        // 禁用玩家控制脚本
        playerScript.enabled = false;
        
        // 处理玩家物理组件
        playerRb = playerScript.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.isKinematic = true; // 或者是 simulated = false，防止物理冲突
            playerRb.velocity = Vector2.zero;
        }

        // 将玩家设为北极熊子物体并移动到座位
        playerScript.transform.SetParent(this.transform);
        if (seatPoint != null)
            playerScript.transform.localPosition = seatPoint.localPosition;
        else
            playerScript.transform.localPosition = Vector3.up * 1f; // 默认骑在头上
            
        // 修正朝向
        playerScript.transform.localRotation = Quaternion.identity;
    }

    void Dismount()
    {
        isRiding = false;
        // 注意：下车依然在碰撞范围内，所以可能立刻要把提示显示回来
        // 为了防止Bug，先把玩家移出一点点或者单纯恢复控制
        
        playerScript.transform.SetParent(null); // 解除父子关系
        
        // 恢复玩家脚本
        playerScript.enabled = true;
        
        // 恢复物理
        if (playerRb != null)
        {
            playerRb.isKinematic = false; 
            // 如果玩家原本 Gravity Scale 是 1，这里不需要改，isKinematic false 就会恢复重力
        }

        // 下车位置微调（防止卡进北极熊身体）
        playerScript.transform.position += Vector3.right * 1.5f;

        // 恢复交互状态
        canInteract = true; // 下车后依然在附近
        if (hintText != null)
        {
            hintText.gameObject.SetActive(true);
            hintText.text = "按J骑，K下车";
        }
    }

    void HandleRidingMovement()
    {
        // 这里控制北极熊本身移动 (简单的左右移动 + 跳跃)
        // 使用 Input.GetAxis 还是 KeyCode 取决于你的习惯，这里复用 WASD 风格
        
        float moveX = 0;
        if (Input.GetKey(KeyCode.D)) moveX = bearSpeed;
        if (Input.GetKey(KeyCode.A)) moveX = -bearSpeed;
        
        // 简单的位移
        transform.Translate(new Vector3(moveX * Time.deltaTime, 0, 0));

        // 如果北极熊也有刚体和跳跃逻辑，可以在这里加
        // 比如北极熊也有 Rigidbody2D
        /*
        if (Input.GetKeyDown(KeyCode.W)) {
             GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
        */
        
        // 简单的翻转朝向，使用初始比例的绝对值，防止覆盖编辑器里的缩放设置
        if (moveX > 0) 
            transform.localScale = new Vector3(Mathf.Abs(initialBearScale.x), initialBearScale.y, initialBearScale.z);
        if (moveX < 0) 
            transform.localScale = new Vector3(-Mathf.Abs(initialBearScale.x), initialBearScale.y, initialBearScale.z);
    }
}
