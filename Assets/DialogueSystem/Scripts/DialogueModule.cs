using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 对话组件(Model与View)
/// </summary>

[RequireComponent(typeof(DialogueTrigger))]
public class DialogueModule : MonoBehaviour,IDialogue
{
    [Header("发起对话者(选项指定人)")]
    public string speakerName;
    public Sprite speakerHead;
    
    [Header("接受对话者")]
    public string listenerName;
    public Sprite listenerHead;
    
    [Header("对话头选项节点")]
    public DialogueSO  headDialogueNode;
    
    [Header("对话框图像")]
    public Sprite dialogBoxSprite;
    
    [Header("对话速度")]
    public float dialogueSpeed = 1f;
    [Header("是否逐字显示")]
    public bool isTypeWriting = true;
    [Header("是否跳过逐字显示")]
    public bool isSkip = false;
    [Header("是否允许跳过对话")]
    public bool isSkipDialogue = false;

    [Header("对话继续按键")] 
    public KeyCode continueKey;
    [Header("对话跳过按键")] 
    public KeyCode skipKey;
    
    [Header("对话模式")]
    public DialogueType dialogueType = DialogueType.双人模式;
    
    [Header("对话开始事件")]
    public UnityEvent dialogueStartEvent;
    
    [Header("对话结束事件")]
    public UnityEvent dialogueEndEvent;

    //当前对话
    private string[] _currentDialogue;//当前对话节点内语句
    private int _currentDialogueIndex;//当前对话节点内语句索引
    
    //当前节点
    private DialogueSO _currentDialogueNode;//当前对话父节点
    private DialogueLinkSO _currentDialogueLinkNode;//当前对话节点
    private OptionSO _currentOptionNode;//当前选项节点
    
    private bool _isWaitingForClick = true;
    private bool _skip;
    private bool _isDialogueEnd = true;

    public void StartDialogue()
    {
        DialogueView.Instance.DialogueViewOpen();
        dialogueStartEvent?.Invoke();
        StartExecution();
    }
    
    private void Start()
    {
        if (dialogBoxSprite != null)
        {
            DialogueView.Instance.dialogBox.sprite = dialogBoxSprite;
            DialogueView.Instance.dialogBox.type = Image.Type.Sliced;
            
            Debug.Log("请划定对话框可拉伸范围");
        }
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void StartExecution()
    {
        _isDialogueEnd = false;
        
        if (headDialogueNode != null)
        {
            _currentDialogueNode = headDialogueNode;
            if (_currentDialogueNode is DialogueLinkSO)
            {
                _currentDialogueLinkNode = _currentDialogueNode as DialogueLinkSO;
                _currentDialogue = _currentDialogueLinkNode.dialogues;
            }
            else
            {
                Debug.LogError("类型错误");
            }

            _isWaitingForClick = false;

            Dialogue();
        }
        else
            Debug.LogError("没有添加头选项节点");
    }
    
    private void Update()
    {
        //未启动对话时，点击无效
        if (_isDialogueEnd)
            return;
        
        //跳过整个对话
        if (Input.GetKeyDown(skipKey) && isSkipDialogue)
        {
            DialogueEnd();
            return;
        }
        
        //跳过当前逐字显示
        if(!_isWaitingForClick && isSkip && isTypeWriting && Input.GetKeyDown(continueKey) && DialogueView.Instance.allowSkipping)
            _skip = true;
        
        DialogueView.Instance.isSkip = _skip;
        
        if (_isWaitingForClick && Input.GetKeyDown(continueKey))
        {
            _isWaitingForClick = false;
            
            //判断当前对话节点内的多个语句是否全部执行完了，如果执行完成那么跳到下一个节点
            if (_currentDialogueIndex < _currentDialogue.Length)
            {
                Dialogue();
            }
            else
            {
                DialogueSwitch();
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Dialogue()
    {
        OptionCheck(listenerHead, listenerName, _currentDialogueLinkNode);
        
        if (isTypeWriting)
        {
            DialogueView.Instance.StartDialogueCoroutine(_currentDialogue[_currentDialogueIndex], () =>
            {
                _currentDialogueIndex++;
                _isWaitingForClick = true;
            },this);
        }
        else
        {
            DialogueView.Instance.StartDialogueDirectlyContinue(_currentDialogue[_currentDialogueIndex], () =>
            {
                _currentDialogueIndex++;
                _isWaitingForClick = true;
            });
        }
    }

    /// <summary>
    /// 语句结束切换函数
    /// </summary>
    // ReSharper disable Unity.PerformanceAnalysis
    private void DialogueSwitch()
    {
        //判断是否到尾节点了
        if (_currentDialogueLinkNode.nextDialogueNodes != null)
        {
            _currentDialogueNode = _currentDialogueLinkNode.nextDialogueNodes;
            //没有到尾节点，就继续判断节点类型，执行不同内容
            if (_currentDialogueNode is DialogueLinkSO)
            {
                _currentDialogueLinkNode = _currentDialogueNode as DialogueLinkSO;
                _currentDialogue = _currentDialogueLinkNode.dialogues;
                _currentDialogueIndex = 0;
                Dialogue();
            }
            else if (_currentDialogueNode is OptionSO)
            {
                _currentOptionNode = _currentDialogueNode as OptionSO;
                Option();
            }
            else
            {
                Debug.LogError("类型错误");
            }
        }
        else
        {
            DialogueView.Instance.DialogueViewClose();
            dialogueEndEvent?.Invoke();
            DialogueEnd();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Option()
    {
        _skip = false;
        _currentDialogueIndex = 0;
        
        DialogueView.Instance.OptionDisplay(_currentOptionNode, this);
    }

    /// <summary>
    /// 跳过逐字显示
    /// </summary>
    public void SkipWordOfWord(Action action)
    {
        action?.Invoke();
        _isWaitingForClick = true;
        _skip = false;
    }
    
    /// <summary>
    /// 选择选项后执行的行动
    /// </summary>
    public void OptionAction(int index)
    {
        OptionCheck(speakerHead, speakerName, _currentOptionNode);
        
        var currentOption = _currentOptionNode.options[index];
        _currentDialogueLinkNode = currentOption.nextDialogueNodes;
        _currentDialogue = _currentDialogueLinkNode.dialogues;
        
        DialogueView.Instance.OptionDisappear(_currentOptionNode);

        if (currentOption.optionDialog.Length == 0 || currentOption.optionDialog == "")
        {
            _currentDialogueNode = currentOption.nextDialogueNodes;
            _isWaitingForClick = true;
            Dialogue();
            return;
        }


        if (isTypeWriting)
            DialogueView.Instance.StartDialogueCoroutine(currentOption.optionDialog, () =>
            {
                _currentDialogueNode = currentOption.nextDialogueNodes;
                _isWaitingForClick = true;
            }, this);
        else
            DialogueView.Instance.StartDialogueDirectlyContinue(currentOption.optionDialog, () =>
            {
                _currentDialogueNode = currentOption.nextDialogueNodes;
                _isWaitingForClick = true;
            });
    }

    /// <summary>
    /// 结束对话
    /// </summary>
    private void DialogueEnd()
    {
        _currentDialogue = null;
        _currentDialogueNode = null;
        _currentDialogueLinkNode = null;
        _currentOptionNode = null;
        
        _currentDialogueIndex = 0;

        _isWaitingForClick = true;
        _isDialogueEnd = true;
    }
    
    /// <summary>
    /// 名字/头像的检查与赋值
    /// </summary>
    private void OptionCheck(Sprite dialogueSprite,string dialogueName, DialogueSO dialogueNode)
    {
        switch (dialogueType)
        {
            case DialogueType.双人模式:
                if (dialogueSprite != null || dialogueName != null)
                {
                    Debug.Log("双人模式，只读取Inspector中的配置");
                }
                DialogueView.Instance.ModeCheck(dialogueSprite, dialogueName);
                break;
            case DialogueType.多人模式:
                if (dialogueNode.sprite != null || dialogueNode.name != null)
                {
                    Debug.Log("多人模式，只读取SO中的配置");
                }
                Debug.Log(DialogueView.Instance);
                DialogueView.Instance.ModeCheck(dialogueNode);
                break;
        }
    }

    public void SwitchDialogueType()
    {
        _isDialogueEnd = false;
    }
}

[Serializable]
public enum DialogueType
{
    双人模式 = 1,
    多人模式 = 2,
}
