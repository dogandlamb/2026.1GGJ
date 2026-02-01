using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理

public class guan2_victory : MonoBehaviour
{
    [Header("设置")]
    public string selectLevelSceneName = "SelectLevelScene"; // 选关场景的名字，请根据实际修改
    public Canvas hintCanvas; // 提示UI

    private bool canFinish = false;

    void Start()
    {
        if (hintCanvas != null) hintCanvas.enabled = false;
    }

    void Update()
    {
        if (canFinish && Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("本关结束，返回选关界面");
            SceneManager.LoadScene(selectLevelSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canFinish = true;
            if (hintCanvas != null) hintCanvas.enabled = true;
            Debug.Log("进入结束区域，按 J 返回");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canFinish = false;
            if (hintCanvas != null) hintCanvas.enabled = false;
        }
    }
}
