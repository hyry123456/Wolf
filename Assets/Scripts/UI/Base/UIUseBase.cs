using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI
{

    /// <summary>
    /// UI���ʹ�õĻ��࣬���е����ʵ���඼Ҫ�̳и���
    /// </summary>
    public class UIUseBase : MonoBehaviour
    {
        /// <summary>        /// ������        /// </summary>
        protected UISceneWidgrt widgrt;
        /// <summary>        /// UI�Ĺ������        /// </summary>
        protected UIControl control;

        /// <summary>
        /// Ĭ�ϳ�ʼ�������������������Լ�UI�Ĺ������,ͬʱֹͣ��UI��ʾ
        /// </summary>
        protected virtual void Awake()
        {
            //��������ı�׼����
            widgrt = GetComponent<UISceneWidgrt>();
            if (widgrt == null)
            {
                widgrt = this.gameObject.AddComponent<UISceneWidgrt>();
            }
            LoadControl();
        }

        /// <summary>
        /// ����UIControl�ķ�������ȡ��������Ϊ��һЩ���岢û��һ��ʼ����������ȷλ��
        /// ������Ҫ�������
        /// </summary>
        public void LoadControl()
        {
            Transform parent = transform.parent;
            Transform preParent = transform;
            while (parent != null)
            {
                preParent = parent;
                parent = parent.parent;
            }
            control = preParent.GetComponent<UIControl>();
        }

        /// <summary>
        /// ��ʾ������, ������Init��ֱ����Ӹ��¼�����ֹ��������������µ�UI��ʧ
        /// </summary>
        protected void ShowSelf()
        {
            UICommon.Instance.ShowUI(this.name, control);
        }


    }
}