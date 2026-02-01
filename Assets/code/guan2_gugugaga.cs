using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guan2_gugugaga : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject[] penguins; // 拖入所有需要下落的企鹅对象
    public guan2_mainCharacter playerScript; // 拖入主角的脚本引用

    private bool hasTriggered = false;
    private int destroyedPenguinCount = 0;
    private const int targetCount = 8;
    private bool maskGiven = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 主角触发逻辑
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            // 1. 播放音频
            if (audioSource != null)
            {
                audioSource.Play();
            }

            // 2. 启用所有企鹅的刚体，让它们下落
            foreach (var penguin in penguins)
            {
                if (penguin != null)
                {
                    Rigidbody2D rb = penguin.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.simulated = true;
                        rb.WakeUp();
                    }
                }
            }
        }
        
        // 企鹅触发逻辑 (企鹅掉落到此触发器或下方另一个销毁触发器)
        // 假设企鹅也有 Tag "Penguin"
        if (other.CompareTag("Penguin"))
        {
            Destroy(other.gameObject);
            destroyedPenguinCount++;
            
            Debug.Log($"销毁企鹅数量: {destroyedPenguinCount}");

            if (destroyedPenguinCount >= targetCount && !maskGiven)
            {
                maskGiven = true;
                GiveQinShiHuangMask();
            }
        }
    }

    void GiveQinShiHuangMask()
    {
        Debug.Log("获得秦始皇面具！");
        if (playerScript != null)
        {
            // 假设秦始皇面具ID是某个特定值，例如3
            // 这里使用了反射或者假设你刚才加了 public 方法，或者直接用 SendMessage
            // 为了稳健，最好在 mainCharacter 里加一个 public 方法
            // 暂时使用 SendMessage 作为一种通用调用方式，或者你可以手动在 mainCharacter 里加个 public void UnlockMask(int id)
             playerScript.SendMessage("getMask", 3, SendMessageOptions.DontRequireReceiver); 
        }
    }
}
