using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选项节点
/// </summary>
[CreateAssetMenu(fileName = "OptionSO", menuName = "DialogueSO/Option")]
public class OptionSO : DialogueSO
{
    public Option[] options;
}

[System.Serializable]
public struct Option
{
    [Header("单选项")]
    public string option;//单选项
    
    [TextArea] [Header("选项对话")]
    public string optionDialog;//选项的拓展语句
    
    [Header("选项后置选项节点")]
    public DialogueLinkSO nextDialogueNodes;//选项后置节点
}
