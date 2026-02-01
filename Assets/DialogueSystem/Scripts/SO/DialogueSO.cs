using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对话节点（父节点）
/// </summary>
public class DialogueSO : ScriptableObject
{
    [Header("名字")]
    public string name;
    [Header("头像")]
    public Sprite sprite;
}
