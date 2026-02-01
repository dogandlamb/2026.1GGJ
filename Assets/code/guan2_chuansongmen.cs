using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guan2_chuansongmen : MonoBehaviour
{
    [Header("设置")]
    public Transform targetPosition; // 拖入你要传送到的目标位置（可以是另一个空的 GameObject）
    public Canvas hintCanvas; // 提示UI

    private bool canTeleport = false;
    private Transform playerTransform;

    void Start()
    {
        if (hintCanvas != null) hintCanvas.enabled = false;
    }

    void Update()
    {
        if (canTeleport && Input.GetKeyDown(KeyCode.J))
        {
            if (targetPosition != null && playerTransform != null)
            {
                playerTransform.position = targetPosition.position;
                Debug.Log("已传送玩家到: " + targetPosition.position);
                
                // 传送后关闭提示（因为玩家已经离开了区域）
                if (hintCanvas != null) hintCanvas.enabled = false;
            }
            else if (targetPosition == null)
            {
                Debug.LogWarning("传送门目标位置 (Target Position) 未设置！");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTeleport = true;
            playerTransform = other.transform;
            if (hintCanvas != null) hintCanvas.enabled = true;
            Debug.Log("进入传送区域，按 J 传送");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTeleport = false;
            playerTransform = null;
            if (hintCanvas != null) hintCanvas.enabled = false;
        }
    }

    // 可选：绘制辅助线方便在编辑器里看连接
    private void OnDrawGizmos()
    {
        if (targetPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPosition.position);
            Gizmos.DrawWireSphere(targetPosition.position, 0.5f);
        }
    }
}
