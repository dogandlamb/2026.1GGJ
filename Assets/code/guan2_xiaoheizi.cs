using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class guan2_xiaoheizi : MonoBehaviour
{
    [Header("角色设置")]
    public guan2_mainCharacter playerScript;
    public Animator bossAnimator; // BOSS（蔡徐坤）的动画机
    public AudioSource bgmSource; // 音乐播放源
    public Animator playerAnimator; // 玩家的动画机（如果需要播放跳舞动作，最好有）
    public int xiaoHeiZiMaskID = 5; // 小黑子面具 ID

    [Header("UI设置")]
    public Canvas gameCanvas; // 整个小游戏的 Canvas
    public GameObject qtePanel;
    public Image arrowImage; // 显示箭头的UI Image
    public TMP_Text feedbackText; // 显示 Perfect/Miss
    public TMP_Text scoreText;

    [Header("资源")]
    public Sprite arrowUp, arrowDown, arrowLeft, arrowRight; // 拖入四个箭头图片

    [Header("游戏参数")]
    public float timeLimit = 1.0f; // 每次QTE的时间限制
    public int targetScore = 5; // 胜利所需连击数

    private bool isGameActive = false;
    private KeyCode currentTargetKey;
    private float timer;
    private int currentScore = 0;

    void Start()
    {
        if (gameCanvas != null) gameCanvas.enabled = false;
        if (qtePanel != null) qtePanel.SetActive(false);
    }

    void Update()
    {
        if (isGameActive)
        {
            timer -= Time.deltaTime;

            if (Input.anyKeyDown)
            {
                CheckInput();
            }

            if (timer <= 0)
            {
                HandleMiss("超时!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isGameActive && other.CompareTag("Player"))
        {
            if (playerScript == null) 
                playerScript = other.GetComponent<guan2_mainCharacter>();

            StartGame();
        }
    }

    void StartGame()
    {
        isGameActive = true;
        currentScore = 0;
        
        if (gameCanvas != null) gameCanvas.enabled = true; // 开启整个 Canvas
        if (qtePanel != null) qtePanel.SetActive(true);
        if (bgmSource != null) bgmSource.Play(); // 播放音乐
        
        if (playerScript != null) playerScript.enabled = false; // 禁用移动
        if (scoreText != null) scoreText.text = "Score: " + currentScore;

        NextRound();
    }

    void NextRound()
    {
        timer = timeLimit;
        
        // 随机生成一个方向 0:Up, 1:Down, 2:Left, 3:Right
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                currentTargetKey = KeyCode.UpArrow; // 也可以兼容WASD
                if (arrowImage != null) arrowImage.sprite = arrowUp;
                break;
            case 1:
                currentTargetKey = KeyCode.DownArrow;
                if (arrowImage != null) arrowImage.sprite = arrowDown;
                break;
            case 2:
                currentTargetKey = KeyCode.LeftArrow;
                if (arrowImage != null) arrowImage.sprite = arrowLeft;
                break;
            case 3:
                currentTargetKey = KeyCode.RightArrow;
                if (arrowImage != null) arrowImage.sprite = arrowRight;
                break;
        }

        // 兼容WASD: 这里逻辑可以优化为检查输入向量，但为了简单直接存KeyCode
        // 也可以用一个变量存 "Up", "Down" 字符串来做多键兼容
        
        if (feedbackText != null) feedbackText.text = "";
        
        // BOSS动画
        if (bossAnimator != null) bossAnimator.SetTrigger("Dance");
    }

    void CheckInput()
    {
        bool hit = false;
        // 简单的输入检查，兼容方向键和WASD
        if (currentTargetKey == KeyCode.UpArrow && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))) hit = true;
        else if (currentTargetKey == KeyCode.DownArrow && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))) hit = true;
        else if (currentTargetKey == KeyCode.LeftArrow && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))) hit = true;
        else if (currentTargetKey == KeyCode.RightArrow && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))) hit = true;
        
        if (hit)
        {
            HandleHit();
        }
        else
        {
            // 如果按下了其他键（不是目标键），判错
            HandleMiss("错啦!");
        }
    }

    void HandleHit()
    {
        currentScore++;
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
        if (feedbackText != null) feedbackText.text = "<color=yellow>Perfect!</color>";

        // 播放对应得瑟动画
        // if (playerAnimator != null) playerAnimator.SetTrigger("Dance");

        if (currentScore >= targetScore)
        {
            WinGame();
        }
        else
        {
            NextRound();
        }
    }

    void HandleMiss(string msg)
    {
        if (feedbackText != null) feedbackText.text = "<color=red>" + msg + "</color>";
        
        // 惩罚
        // 按实际需求：可以扣血重来，或者仅扣分
        // 这里演示：扣分不扣血，或者直接重置当前轮
        // currentScore = Mathf.Max(0, currentScore - 1);
        
        // 播放摔倒动画
        // if (playerAnimator != null) playerAnimator.SetTrigger("Fall");

        NextRound(); // 即使错了也进入下一轮，保持节奏
    }

    void WinGame()
    {
        isGameActive = false;
        if (bgmSource != null) bgmSource.Stop(); // 停止音乐
        if (feedbackText != null) feedbackText.text = "<color=green>WIN!</color>";
        
        // 奖励面具
        if (playerScript != null)
        {
            playerScript.getMask(xiaoHeiZiMaskID);
            playerScript.enabled = true; // 恢复移动
        }

        StartCoroutine(ClosePanelDelay());
    }

    IEnumerator ClosePanelDelay()
    {
        yield return new WaitForSeconds(2f);
        if (gameCanvas != null) gameCanvas.enabled = false; // 隐藏 Canvas
        if (qtePanel != null) qtePanel.SetActive(false);
        Destroy(this); // 销毁脚本或物体，防止重复触发
    }
}
