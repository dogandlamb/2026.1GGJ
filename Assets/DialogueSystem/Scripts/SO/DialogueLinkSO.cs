using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对话链节点
/// </summary>
[CreateAssetMenu(fileName = "DialogueLinkSO", menuName = "DialogueSO/DialogueLink")]
public class DialogueLinkSO : DialogueSO
{
    [TextArea]
    public string[] dialogues;
    
    public DialogueSO nextDialogueNodes;//后置节点
}
