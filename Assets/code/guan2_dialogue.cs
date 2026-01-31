using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guan2_dialogue : MonoBehaviour
{
    [Header("UI组件")]
    public Canvas dialogueCanvas; // 拖入包含对话和选项按钮的 Canvas

    [Header("玩家设置")]
    public guan2_mainCharacter playerScript; // 拖入主角对象
    public int dontCareMaskID = 4; // "不鸟你面具"的ID，请根据实际情况修改
    
    private bool hasAnsweredCorrectly = false;

    void Start()
    {
        // 确保游戏开始时Canvas是关闭的
        if (dialogueCanvas != null)
            dialogueCanvas.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只有未答对过才显示 Canvas
        if (!hasAnsweredCorrectly && other.CompareTag("Player") && dialogueCanvas != null)
        {
            dialogueCanvas.enabled = true; // 进入区域开启 Canvas
            Debug.Log("进入区域，开启对话面板");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dialogueCanvas != null)
        {
            dialogueCanvas.enabled = false; // 离开区域关闭 Canvas
        }
    }

    // 绑定到 按钮A 的 OnClick 事件
    public void OnOptionAClicked()
    {
        if (playerScript != null)
        {
            Debug.Log("选择了A：获得面具");
            playerScript.getMask(dontCareMaskID);
            
            // 标记为已答对
            hasAnsweredCorrectly = true;

            // 获得奖励后可以选择关闭 Canvas 或者显示感谢信息
            dialogueCanvas.enabled = false; 
            
            // 也可以选择在这里禁用碰撞体，防止之后再检测
            // GetComponent<Collider2D>().enabled = false;
        }
    }

    // 绑定到 按钮B 和 按钮C 的 OnClick 事件
    public void OnWrongOptionClicked()
    {
        if (playerScript != null)
        {
            Debug.Log("选错了：扣血");
            playerScript.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
            // 扣血后也可以选择关闭 Canvas
            dialogueCanvas.enabled = false;
        }
    }
}
