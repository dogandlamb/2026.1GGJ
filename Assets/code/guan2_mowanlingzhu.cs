using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 必须引用UI库用于Slider

public class guan2_mowanlingzhu : MonoBehaviour
{
    [Header("角色与面具")]
    public guan2_mainCharacter playerScript;
    public int mowanMaskID = 6; // 魔丸灵珠面具的ID

    [Header("鼎设置")]
    public Transform cauldronCenter; // 鼎的中心位置（把玩家吸过去）
    public GameObject effectCooking; // “被煮”特效
    public GameObject effectExplosion; // “爆开”特效

    [Header("UI")]
    public Canvas gameCanvas; // 整个小游戏的 Canvas
    public GameObject escapePanel; // 包含进度条和提示文字的Panel
    public Slider progressSlider; // 进度条

    [Header("参数")]
    public float decayRate = 30f; // 进度自动衰减速度
    public float pressGain = 15f; // 每次按空格增加的进度
    public float maxProgress = 100f;

    private bool isTrapped = false;
    private float currentProgress = 0f;

    void Start()
    {
        if (gameCanvas != null) gameCanvas.enabled = false;
        if (escapePanel != null) escapePanel.SetActive(false);
        if (effectCooking != null) effectCooking.SetActive(false);
    }

    void Update()
    {
        if (isTrapped)
        {
            // 连按逻辑
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentProgress += pressGain;
            }

            // 自动衰减
            currentProgress -= decayRate * Time.deltaTime;
            
            // 钳制进度值在 0 到 100 之间
            currentProgress = Mathf.Clamp(currentProgress, 0f, maxProgress);

            // 更新UI
            if (progressSlider != null) 
                progressSlider.value = currentProgress / maxProgress;

            // 成功检测
            if (currentProgress >= maxProgress)
            {
                BreakFree();
            }

            // 只要被抓住，时刻强制将玩家拉回中心（防止物理挤出去）
            if (playerScript != null && cauldronCenter != null)
            {
                playerScript.transform.position = cauldronCenter.position;
                playerScript.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 甚至可以锁死刚体
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTrapped && other.CompareTag("Player"))
        {
            if (playerScript == null)
                playerScript = other.GetComponent<guan2_mainCharacter>();

            TrapPlayer();
        }
    }

    void TrapPlayer()
    {
        isTrapped = true;
        currentProgress = 0f;

        // 禁用玩家移动
        if (playerScript != null) playerScript.enabled = false;
        
        // 开启UI
        if (gameCanvas != null) gameCanvas.enabled = true;
        if (escapePanel != null) escapePanel.SetActive(true);
        
        // 开启特效
        if (effectCooking != null) effectCooking.SetActive(true);
    }

    void BreakFree()
    {
        isTrapped = false;
        Debug.Log("挣脱成功！获得面具！");

        // 播放爆炸特效
        if (effectExplosion != null) 
        {
            Instantiate(effectExplosion, transform.position, Quaternion.identity);
            // 或者 effectExplosion.SetActive(true);
        }

        // 获得面具
        if (playerScript != null)
        {
            playerScript.getMask(mowanMaskID);
            playerScript.enabled = true; // 恢复控制
        }

        // 关闭UI和煮的特效
        if (gameCanvas != null) gameCanvas.enabled = false;
        if (escapePanel != null) escapePanel.SetActive(false);
        if (effectCooking != null) effectCooking.SetActive(false);

        // 销毁鼎自身或者触发器，防止再次被抓
        // Destroy(gameObject); // 直接销毁
        // 或者只禁用 Collider
        GetComponent<Collider2D>().enabled = false;
    }
}
