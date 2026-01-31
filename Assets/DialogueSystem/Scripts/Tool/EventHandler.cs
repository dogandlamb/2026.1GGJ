using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action StartDialogueEvent; //定义事件
    public static void CallStartDialogueEvent() 
    {
        StartDialogueEvent?.Invoke(); //激活事件注册的回调函数
    }
    
    public static event Action FinishDialogueEvent;
    public static void CallFinishDialogueEvent() 
    {
        FinishDialogueEvent?.Invoke();
    }
}
