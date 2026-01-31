using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

[DefaultExecutionOrder(-100)]//优先执行脚本
public class OptionsPool : MonoBehaviour
{
    public GameObject optionPrefab;
    private ObjectPool<GameObject> pool;
    public Transform optionTrans;

    public int maxSize = 10;
    
    private void Awake()
    {
        optionPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/DialogueSystem/Prefabs/Option.prefab", typeof(GameObject));
        
        //初始化对象池
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(optionPrefab,optionTrans),//创建
            actionOnGet: (obj) => obj.SetActive(true),//获取
            actionOnRelease: (obj) => obj.SetActive(false),//回收（到池中）
            actionOnDestroy: (obj) => Destroy(obj),//删除
            collectionCheck: false,
            defaultCapacity: 3,//默认数量
            maxSize: maxSize //最大数量
        );
    }

    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
