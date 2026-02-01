using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class guan2_UI : MonoBehaviour
{
    [Header("玩家引用")]
    public guan2_mainCharacter player;

    [Header("生命值设置")]
    public TMP_Text healthText; // 用于显示 "X3" 的文本

    [Header("面具栏设置")]
    // 该关卡主要涉及的4个面具ID：3(秦始皇), 4(不鸟你), 5(小黑子), 6(魔丸)
    public int[] displayMaskIDs = new int[] { 3, 4, 5, 6 };
    public MaskSlotUI[] maskSlots; // 对应UI上的4个格子

    [Header("声音设置")]
    public AudioSource audioSource;
    public AudioClip sfxButtonClick;

    [Header("指针设置")]
    public RectTransform pointer; // 指针图片
    public float pointerYOffset = 50f; // 指针在面具上方的偏移量

    [System.Serializable]
    public class MaskSlotUI
    {
        public Image iconImage; // 面具图标
        public TMP_Text nameText; // 面具名字
        [HideInInspector]
        public RectTransform rectTransform; // 自动获取
    }

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        // 初始化获取每个槽位的 RectTransform，方便移动指针
        for (int i = 0; i < maskSlots.Length; i++)
        {
            if (maskSlots[i].iconImage != null)
            {
                maskSlots[i].rectTransform = maskSlots[i].iconImage.GetComponent<RectTransform>();
            }
        }
    }

    // 供UI按钮绑定的公共方法
    public void PlayClickSound()
    {
        if (audioSource != null && sfxButtonClick != null)
        {
            audioSource.PlayOneShot(sfxButtonClick);
        }
    }

    void Update()
    {
        if (player == null) return;

        UpdateHealth();
        UpdateMasks();
        UpdatePointer();
    }

    void UpdateHealth()
    {
        if (healthText != null)
        {
            healthText.text = "X" + player.health.ToString();
        }
    }

    void UpdateMasks()
    {
        for (int i = 0; i < displayMaskIDs.Length; i++)
        {
            int maskID = displayMaskIDs[i];
            bool hasMask = player.HasMask(maskID); // 使用之前加的 HasMask 方法

            // 越界检查
            if (i >= maskSlots.Length) break;
            
            var slot = maskSlots[i];
            if (slot.iconImage != null)
            {
                // 如果拥有，显示正常颜色；未拥有，显示灰色/黑色
                slot.iconImage.color = hasMask ? Color.white : Color.black;
            }
            
            if (slot.nameText != null)
            {
                slot.nameText.color = hasMask ? Color.white : Color.grey;
            }
        }
    }

    void UpdatePointer()
    {
        if (pointer == null) return;

        int targetSlotIndex = 0; // 默认指向第0个插槽（即ID为3的面具）

        for (int i = 0; i < displayMaskIDs.Length; i++)
        {
            // 如果玩家当前选中的 maskType 等于我们展示列表里的 ID (例如3, 4, 5, 6)
            if (player.maskType == displayMaskIDs[i])
            {
                targetSlotIndex = i;
                break;
            }
        }
        
        // 如果没找到匹配项（例如 maskType 是 0, 1, 2 等不在列表里的），
        // 且 displayMaskIDs 第一个就是 3，默认指向它
        // 注意：这取决于你希望 maskType=0,1,2 时指针指哪里。
        // 你说“别轮询012了，从3开始”，我理解是如果 maskType 不是 3~6，
        // 就默认让它看起来像是指向 3 (列表的第0个) 或者不做特殊对齐但保持在起始位置。
        
        // 无论是否匹配到，都让指针显示
        pointer.gameObject.SetActive(true);

        // 如果在列表中找到了对应面具，指向它；没找到（比如默认0状态），就指向默认的第一个位置
        if (targetSlotIndex < maskSlots.Length && maskSlots[targetSlotIndex].rectTransform != null)
        {
            if (maskSlots[targetSlotIndex].rectTransform != null)
            {
                Vector3 targetPos = maskSlots[targetSlotIndex].rectTransform.position;
                pointer.position = targetPos + Vector3.up * pointerYOffset; 
            }
        }
    }
}
