using UnityEngine;

namespace Control
{
    /// <summary>
    /// ��ɫ��������Ҫ�̳еĻ��࣬���������ȷ���ĸ������ǣ�
    /// ���ṩʲô������������ֻ��������ȡ�����õ�
    /// </summary>
    public abstract class ControlBase : MonoBehaviour
    {
        protected static ControlBase instance;
        public static ControlBase Instance
        {
            get
            {
                return instance;
            }
        }

        public static void ChangeToPlayer(ControlBase playerControl)
        {
            Instance.gameObject.SetActive(false);
            playerControl.gameObject.SetActive(true);
        }

        protected bool isEnableInput;     //�����ж��Ƿ�������

        /// <summary>   /// ����Ϊ��������    /// </summary>
        public void EnableInput()
        {
            isEnableInput = true;
        }
        /// <summary>   /// ����Ϊ��ֹ����    /// </summary>
        public void DisableInput()
        {
            isEnableInput = false;
        }

        /// <summary>       /// ��ȡ����λ��      /// </summary>
        public abstract Vector3 GetPosition();


    }
}