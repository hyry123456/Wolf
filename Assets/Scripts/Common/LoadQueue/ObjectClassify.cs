using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 颜色分类结构体，用来确定所有需要分类的物体
/// </summary>
public class ObjectClassify : MonoBehaviour
{
    public static ObjectClassify instance;
    public static ObjectClassify Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject game = new GameObject("ObjectClassify");
                game.AddComponent<ObjectClassify>();
            }
            return instance;
        }
    }

    private string[] allClassify = new string[]
    {
        "White", "Black"
    };
    //public List<List<GameObject>> allObjects;
    public Dictionary<string, List<GameObject>> allObjects;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        allObjects = new Dictionary<string, List<GameObject>>();
        for(int i=0; i<allClassify.Length; i++)
        {
            List<GameObject> objects = new List<GameObject>( GameObject.FindGameObjectsWithTag(allClassify[i]));
            allObjects.Add(allClassify[i], objects);
        }

    }

    private void OnDestroy()
    {
        instance = null;
        allObjects.Clear();
    }
}
