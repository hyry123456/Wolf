using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// UI����������������һϵ�е�UI������һ��������,���ͨ��һ�Ż���ֻ��һ��UIScene��
    /// ����Ҫ������Ӧ����ֻ��Ҫ���Ҹ����������
    /// </summary>
    public class UIControl : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>        /// UI����Ϸ�����ֵ�        /// </summary>
        public Dictionary<string, GameObject> UIObjectDictionary;
        public delegate void Init();
        /// <summary>        /// ��ʼ����������ֹһЩ��ʼ������ʱ�������ʼ�������ֵ���غ���õ�        /// </summary>
        public Init init;

        public delegate void SceneClick(PointerEventData eventData);
        private SceneClick sceneClick;


        /// <summary>    /// ��ʼ���ռ䣬��û����������ʱ��    /// </summary>
        private void Awake()
        {
            UIObjectDictionary = new Dictionary<string, GameObject>();
        }

        private IEnumerator Start()
        {
            yield return null;
            ReadyAllUIWidget(this.transform);
            if(init != null)
            {
                init();
            }
            init = null;
        }

        /// <summary>
        /// �������е���ⲿ�������������е������,ͬʱ���ڻ�仯�������
        /// �����ȡ����ʾ����Ҫ��ʾʱ���ⲿ������ʾ��������
        /// </summary>
        private void ReadyAllUIWidget(Transform transform)
        {
            UISceneWidgrt uiSceneWidgrt;
            uiSceneWidgrt = transform.GetComponent<UISceneWidgrt>();
            ISceneClickHandler pointClick = transform.GetComponent<ISceneClickHandler>();
            if(uiSceneWidgrt != null)
            {
                UIObjectDictionary.Add(transform.name, transform.gameObject);
                transform.gameObject.SetActive(false);
                //������������ĵ���¼�
                sceneClick += pointClick.ScenePointClick;
            }
            int count = transform.childCount;
            for(int i=0; i<count; i++)
            {
                ReadyAllUIWidget(transform.GetChild(i));
            }
        }

        /// <summary>
        /// �����ܵ����ʱ����
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (sceneClick != null)
                sceneClick(eventData);
        }

        /// <summary>
        /// ��ӳ������ʱ�䣬�ú���������������ӵ�������ó�������¼�
        /// </summary>
        /// <param name="scene"></param>
       public void AddSceneClick(SceneClick scene)
       {
            sceneClick += scene;
       }

        /// <summary>
        /// ���ڸ���UI�Ƴ�ʱ���õķ����������Ƴ�����¼�
        /// </summary>
        /// <param name="scene"></param>
        public void DeleteSceneClick(SceneClick scene)
        {
            sceneClick -= scene;
        }

    }
}