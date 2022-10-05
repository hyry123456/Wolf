using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>    
    /// 场景的组件管理类，用来用名称找到物体
    /// </summary>
    public class SceneObjectMap : MonoBehaviour
    {
        private static SceneObjectMap instance;
        public static SceneObjectMap Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject go = new GameObject("SceneObjectMap");
                    instance = go.AddComponent<SceneObjectMap>();
                }
                return instance;
            }
        }
        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            LoadAllObject();
        }

        private void OnDestroy()
        {
            instance = null;
            objectMap.Clear();
        }
        /// <summary>
        /// 控制物体，这个物体是一开始就在场景中存在的物体，不过在一些时候需要提供交互
        /// 这种时候就添加该标签
        /// </summary>
        const string controlName = "ControlObject";
        /// <summary>
        /// 临时物体，这个物体是一开始不显示在场景中的，但是在某些任务需要该物体时，
        /// 可以进行快速获取，也就是说这个物体一开始是显示在场景中的，但是在开始时被关闭了
        /// </summary>
        const string tempName = "TempObject";
        Dictionary<string, GameObject> objectMap;

        void LoadAllObject()
        {
            GameObject[] controlObjects = GameObject.FindGameObjectsWithTag(controlName);
            GameObject[] tempObjects = GameObject.FindGameObjectsWithTag(tempName);
            objectMap = new Dictionary<string, GameObject>(controlObjects.Length + tempObjects.Length);
            for(int i=0; i< controlObjects.Length; i++)
            {
                objectMap.Add(controlObjects[i].name, controlObjects[i]);
            }

            for (int i = 0; i < tempObjects.Length; i++)
            {
                objectMap.Add(tempObjects[i].name, tempObjects[i]);
                tempObjects[i].SetActive(false);        //临时物体一开始不进行显示
            }
        }

        public GameObject FindControlObject(string name)
        {
            GameObject obj = null;
            if (objectMap.TryGetValue(name, out obj)) { }
            return obj;
        }

        public void ReleaseObject()
        {
            objectMap.Clear();
        }
    }
}