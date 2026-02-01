using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Credits : MonoBehaviour
{
    [System.Serializable]
    public class AuthorData
    {
        [Header("名字/昵称 (文字或图片)")]
        public string name; // 开发者名字
        public Sprite nameImage; // 名字图片 (有图优先)
        
        public Sprite portrait; // 立绘
        
        [Header("Role (通过 文字 或 图片 显示)")]
        [TextArea] public string roles; // 职位文字
        public Sprite roleImage; // 职位图片 (如果有图，优先显示图)

        [Header("Message (通过 文字 或 图片 显示)")]
        [TextArea] public string message; // 寄语文字
        public Sprite messageImage; // 寄语图片 (如果有图，优先显示图)

        [Header("可选：背景/字体设置")]
        public TMP_FontAsset fontForText; 
        public Sprite roleBoxBackground; 
        public Sprite messageBoxBackground; 
    }

    [Header("作者数据 (请配置7位作者)")]
    public List<AuthorData> authors = new List<AuthorData>();

    [Header("UI 组件引用")]
    public Image portraitImage; // 左边的立绘框
    
    [Header("Role 部分的组件")]
    public TMP_Text roleText;   // 显示Role文字
    public Image roleImageDisplay; // 显示Role图片 (需在场景创建Image并拖入)
    public Image roleBoxBg;     // Role背景

    [Header("Name 部分的组件")]
    public TMP_Text nameText;   // 显示名字文字
    public Image nameImageDisplay; // 显示名字图片 

    [Header("Message 部分的组件")]
    public TMP_Text messageText; // 显示Message文字
    public Image messageImageDisplay; // 显示Message图片 (需在场景创建Image并拖入)
    public Image messageBoxBg;   // Message背景

    [Header("默认设置 (用于回退)")]
    public TMP_FontAsset defaultFont; 
    public Sprite defaultRoleBoxBg; // 默认Role背景 (新加)
    public Sprite defaultMessageBoxBg;

    private int currentIndex = 0;

    void Start()
    {
        // 记录一下初始状态作为默认状态，防止切来切去乱了
        if (defaultFont == null && roleText != null) defaultFont = roleText.font;
        // 如果 roleText 没拿到，试试拿 nameText 的字体
        if (defaultFont == null && nameText != null) defaultFont = nameText.font;
        
        if (defaultRoleBoxBg == null && roleBoxBg != null) defaultRoleBoxBg = roleBoxBg.sprite;
        if (defaultMessageBoxBg == null && messageBoxBg != null) defaultMessageBoxBg = messageBoxBg.sprite;

        // 初始化显示第一位
        ShowAuthor(currentIndex);
    }

    // 绑定到右箭头按钮
    public void OnNextClick()
    {
        currentIndex++;
        if (currentIndex >= authors.Count)
        {
            currentIndex = 0; // 循环到第一个
        }
        ShowAuthor(currentIndex);
    }

    // 绑定到左箭头按钮
    public void OnPrevClick()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = authors.Count - 1; // 循环到最后一个
        }
        ShowAuthor(currentIndex);
    }

    void ShowAuthor(int index)
    {
        if (authors.Count == 0) return;

        // 安全检查
        if (index < 0) index = 0;
        if (index >= authors.Count) index = authors.Count - 1;

        AuthorData data = authors[index];

        if (portraitImage != null)
        {
            // 如果有图就显示，没图可以设为透明或者默认图，这里假设都有图
            if (data.portrait != null) 
            {
                portraitImage.sprite = data.portrait;
                portraitImage.gameObject.SetActive(true);
            }
            else
            {
                // portraitImage.gameObject.SetActive(false); // 或者保留空框
            }
        }

        // --- Name 部分显示逻辑 ---
        if (data.nameImage != null && nameImageDisplay != null)
        {
             // 显示图片，隐藏文字
            nameImageDisplay.sprite = data.nameImage;
            nameImageDisplay.gameObject.SetActive(true);
            nameImageDisplay.preserveAspect = true;
            if (nameText != null) nameText.gameObject.SetActive(false);
        }
        else
        {
            // 显示文字
            if (nameImageDisplay != null) nameImageDisplay.gameObject.SetActive(false);
            if (nameText != null)
            {
                nameText.gameObject.SetActive(true);
                nameText.text = data.name;
                // 复用 fontForText
                nameText.font = (data.fontForText != null) ? data.fontForText : defaultFont;
            }
        }
        
        // --- Role 部分显示逻辑 ---
        // 优先检查是否有图片
        if (data.roleImage != null && roleImageDisplay != null)
        {
            // 显示图片，隐藏文字
            roleImageDisplay.sprite = data.roleImage;
            roleImageDisplay.gameObject.SetActive(true);
            if(roleText != null) roleText.gameObject.SetActive(false);
        }
        else
        {
            // 显示文字，隐藏图片
            if(roleImageDisplay != null) roleImageDisplay.gameObject.SetActive(false);
            if(roleText != null) 
            {
                roleText.gameObject.SetActive(true);
                roleText.text = data.roles;
                roleText.font = (data.fontForText != null) ? data.fontForText : defaultFont;
            }
        }

        // --- Message 部分显示逻辑 ---
        if (data.messageImage != null && messageImageDisplay != null)
        {
            // 显示图片，隐藏文字
            messageImageDisplay.sprite = data.messageImage;
            messageImageDisplay.gameObject.SetActive(true);
            messageImageDisplay.preserveAspect = true; // 保持图片比例
            if(messageText != null) messageText.gameObject.SetActive(false);
        }
        else
        {
            // 显示文字，隐藏图片
            if(messageImageDisplay != null) messageImageDisplay.gameObject.SetActive(false);
            if(messageText != null)
            {
                messageText.gameObject.SetActive(true);
                messageText.text = data.message;
                messageText.font = (data.fontForText != null) ? data.fontForText : defaultFont;
            }
        }

        // 更新 Role 框背景
        if (roleBoxBg != null)
        {
            roleBoxBg.sprite = (data.roleBoxBackground != null) ? data.roleBoxBackground : defaultRoleBoxBg;
        }

        // 更新 Message 框背景
        if (messageBoxBg != null)
        {
            messageBoxBg.sprite = (data.messageBoxBackground != null) ? data.messageBoxBackground : defaultMessageBoxBg;
        }
    }
}
