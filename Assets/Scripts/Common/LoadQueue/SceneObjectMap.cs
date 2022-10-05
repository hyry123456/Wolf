using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>    
    /// ��������������࣬�����������ҵ�����
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
        /// �������壬���������һ��ʼ���ڳ����д��ڵ����壬������һЩʱ����Ҫ�ṩ����
        /// ����ʱ�����Ӹñ�ǩ
        /// </summary>
        const string controlName = "ControlObject";
        /// <summary>
        /// ��ʱ���壬���������һ��ʼ����ʾ�ڳ����еģ�������ĳЩ������Ҫ������ʱ��
        /// ���Խ��п��ٻ�ȡ��Ҳ����˵�������һ��ʼ����ʾ�ڳ����еģ������ڿ�ʼʱ���ر���
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
                tempObjects[i].SetActive(false);        //��ʱ����һ��ʼ��������ʾ
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