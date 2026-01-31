using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间

public class guan2_bluebird : MonoBehaviour
{
    [Header("设置")]
    public guan2_mainCharacter playerScript;
    public int dontCareMaskID = 4; // 必须和之前的ID一致
    public GameObject wallToOpen; // 成功后要消除的墙/障碍物

    [Header("UI")]
    public Canvas inputCanvas;
    public TMP_Text hintText; // 用于显示提示信息
    public TMP_InputField inputField;
    public TMP_Text timerText;

    private const string TARGET_PHRASE = "我鸟都不鸟你";

    void Start()
    {
        if (inputCanvas != null) inputCanvas.enabled = false;
        if (inputField != null) 
        {
            inputField.onEndEdit.AddListener(CheckInput); // 绑定输入结束事件
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (inputCanvas != null) inputCanvas.enabled = true;
            
            // 下面这几行是重置UI状态
            if (hintText != null) hintText.text = "请输入...";
            if (inputField != null) 
            {
                inputField.text = ""; 
                inputField.ActivateInputField(); 
            }
            if (timerText != null) timerText.text = ""; // 如果不需要倒计时可以清空或隐藏
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (inputCanvas != null) inputCanvas.enabled = false;
        }
    }

    // 绑定到 InputField 的 OnEndEdit
    void CheckInput(string input)
    {
        // 如果输入不对，或者 Canvas 没开（玩家不在区域内），直接返回
        if (inputCanvas == null || !inputCanvas.enabled) return;

        if (input == TARGET_PHRASE)
        {
            CheckMask();
        }
        else
        {
            // 输入错误的内容（可选提示）
             if (hintText != null) hintText.text = "输入错误，请输入：\n" + TARGET_PHRASE;
        }
    }

    void CheckMask()
    {
        if (playerScript == null) return;

        int currentMask = playerScript.GetCurrentMaskType();

        if (currentMask == dontCareMaskID)
        {
            // 成功：戴了面具且输入正确
            Debug.Log("成功！破坏障碍物");
            if (wallToOpen != null) Destroy(wallToOpen);
            
            // 关闭UI
            if (inputCanvas != null) inputCanvas.enabled = false;
        }
        else
        {
            // 失败：没戴面具
            if (hintText != null) hintText.text = "请戴上 <color=red>不鸟你面具</color> 再试！";
        }
    }
}
