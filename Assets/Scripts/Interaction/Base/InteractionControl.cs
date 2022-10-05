
using UnityEngine;

namespace Interaction
{
    public class InteractionControl : MonoBehaviour
    {
        private static InteractionControl instance;

        public static InteractionControl Instance
        {
            get
            {
                return instance;
            }
        }

        private Control.PlayerControl playerControl;
        /// <summary>        /// ��ǰ���Դ����Ľ�����Ϣ        /// </summary>
        public InteractionBase nowInteractionInfo;
        /// <summary>        /// �Ƿ����ڽ�����        /// </summary>
        public bool isInteracting;
        /// <summary>        /// ���߼��ľ���        /// </summary>
        public float interacteCheckDistance = 3f;

        private InteractionControl() { }

        private void OnEnable()
        {
            instance = this;
        }

        private void OnDisable()
        {
            instance = null;
        }

        /// <summary>        /// �ҵ������ϣ�������ʱʱ����        /// </summary>
        private void Start()
        {
            playerControl = gameObject.GetComponent<Control.PlayerControl>();
            isInteracting = false;
        }

        /// <summary>        /// ���н����¼�        /// </summary>
        /// <param name="interactionInfo">�����Ľ����¼�</param>
        public void RunInteraction(InteractionBase interactionInfo)
        {
            isInteracting = true;   //�������ڽ���
            if (interactionInfo == null) { 
                Debug.Log("�����������");
            }
            //���н�����Ϊ
            interactionInfo.InteractionBehavior();
        }

        /// <summary>
        /// ���е�ǰ���ڽ����Ľ����¼�
        /// </summary>
        public void RunInteraction()
        {
            if (nowInteractionInfo == null) return;
            RunInteraction(nowInteractionInfo);
        }

        /// <summary>   /// ��ʾֹͣ��������ϵͳ���¿�ʼ����    /// </summary>
        public void StopInteraction()
        {
            isInteracting = false;
        }

        /// <summary>        /// ��������        /// </summary>
        public void StartInteraction()
        {
            isInteracting = true;
        }

        public bool GetInteraction()
        {
            return isInteracting;
        }

        /// <summary>
        /// ���һ�������¼�����Ϊ2D�Ľ�����Ϊ����ʽ��
        /// </summary>
        public void AddInteraction(InteractionBase interaction)
        {
            nowInteractionInfo = interaction;
        }

        /// <summary>
        /// �����ǰ�Ľ����¼�
        /// </summary>
        public void RemoveInteraction()
        {
            nowInteractionInfo = null;
        }

    }
}