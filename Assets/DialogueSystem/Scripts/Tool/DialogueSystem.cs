using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueSystem : EditorWindow
{
    private static DialogueSystem window; //窗口实例对象，必须是一个static
    private static GameObject prefab;

    [MenuItem("DialogueSystem/CreateDialogueSystem")] //定义菜单栏位置
    public static void OpenDialogueWindow() //打开窗口函数，必须是static
    {
        prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/DialogueSystem/Prefabs/DialogueSystem.prefab", typeof(GameObject));
        Instantiate(prefab);
    }
    
    [MenuItem("DialogueSystem/CreateNPC")] //定义菜单栏位置
    public static void OpenNPCWindow() //打开窗口函数，必须是static
    {
        prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/DialogueSystem/Prefabs/NPC.prefab", typeof(GameObject));
        Instantiate(prefab);
    }
}
