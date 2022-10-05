
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// UI ��������������������Լ�����UI��ʵʱ��������ʾ�����һ����仯��UI
    /// </summary>
    public class UISceneWidgrt : MonoBehaviour, IPointerClickHandler, IDragHandler, 
        IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, ISceneClickHandler, IPointerUpHandler
    {
        public delegate void PointerClick(PointerEventData eventData);
        /// <summary>        /// ������¼�        /// </summary>
        public PointerClick pointerClick;
        /// <summary>
        /// ������¼��ĵ��ã��벻Ҫ����޸ĸú�������ӵ���¼�ֱ���ڹ�����¼�����Ӽ���
        /// </summary>
        /// <param name="eventData">�������</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (pointerClick != null)
                pointerClick(eventData);
        }

        public delegate void Drag(PointerEventData eventData);
        /// <summary>        /// ��ק�¼�        /// </summary>
        public Drag drag;
        /// <summary>       
        /// /// ��ק�¼����ã��벻Ҫ�����øú������¼������drag����Ӽ���   
        /// /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            if (drag != null)
                drag(eventData);
        }

        public delegate void PointerDown(PointerEventData eventData);
        /// <summary>        /// ��갴���¼�������갴��ʱ����        /// </summary>
        public PointerDown pointerDown;
        /// <summary>        
        /// /// ��갴���¼����ã�����pointerDown������¼�        
        /// /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (pointerDown != null)
                pointerDown(eventData);
        }

        /// <summary>        /// ����ɿ��¼�����        /// </summary>
        public delegate void PointerUp(PointerEventData eventData);
        /// <summary>        /// ����ɿ��¼�        /// </summary>
        public PointerUp pointerUp;
        public void OnPointerUp(PointerEventData eventData)
        {
            if(pointerUp != null)
            {
                pointerUp(eventData);
            }
        }

        public delegate void PointerEnter(PointerEventData eventData);
        /// <summary>        /// �������¼�����������ʱ����        /// </summary>
        public PointerEnter pointerEnter;
        /// <summary>        
        /// /// �������¼��ĵ��ã�����pointerEnter������¼�       
        /// /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (pointerEnter != null)
                pointerEnter(eventData);
        }

        /// <summary>        /// ����˳��¼�        /// </summary>
        public delegate void PointerExit(PointerEventData eventData);
        public PointerExit pointerExit;
        /// <summary>        /// ����˳��¼����ú������������        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (pointerExit != null)
            {
                pointerExit(eventData);
            }
        }

        public delegate void SceneClick(PointerEventData eventData);
        public SceneClick sceneClick;
        public void ScenePointClick(PointerEventData eventData)
        {
            if (sceneClick != null)
                sceneClick(eventData);
        }

        //private void Awake()
        //{
        //    gameObject.SetActive(false);
        //}

    }
}