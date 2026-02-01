using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理
using TMPro;

public class guan2_tongguan : MonoBehaviour
{
    [Header("设置")]
    public guan2_mainCharacter playerScript;
    public string nextSceneName = "WinScene"; // 通关后跳转的场景名

    [Header("面具检查")]
    // 假设你需要收集的4个面具ID是 3(秦始皇), 4(不鸟你), 5(小黑子), 6(魔丸)
    // 请根据你的实际ID调整
    public int[] requiredMaskIDs = new int[] { 3, 4, 5, 6 }; 

    [Header("UI")]
    public Canvas infoCanvas;
    public TMP_Text infoText;

    private bool isPlayerInZone = false;
    private bool allMasksCollected = false;

    void Start()
    {
        if (infoCanvas != null) infoCanvas.enabled = false;
    }

    void Update()
    {
        // 只有在区域内且收集齐了，按 J 才能通关
        if (isPlayerInZone && allMasksCollected && Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("通关！");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (playerScript == null) 
                playerScript = other.GetComponent<guan2_mainCharacter>();

            CheckMasks();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (infoCanvas != null) infoCanvas.enabled = false;
        }
    }

    void CheckMasks()
    {
        if (playerScript == null) return;

        allMasksCollected = true;
        int missingCount = 0;

        foreach (int id in requiredMaskIDs)
        {
            if (!playerScript.HasMask(id))
            {
                allMasksCollected = false;
                missingCount++;
            }
        }

        if (infoCanvas != null)
        {
            infoCanvas.enabled = true;
            if (infoText != null)
            {
                if (allMasksCollected)
                {
                    infoText.text = "恭喜集齐所有面具！\n按 <color=yellow>J</color> 通关";
                }
                else
                {
                    infoText.text = $"还缺 <color=red>{missingCount}</color> 个面具\n无法通过";
                }
            }
        }
    }
}
