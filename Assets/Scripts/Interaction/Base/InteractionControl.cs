
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
        /// <summary>        /// 当前可以触发的交互信息        /// </summary>
        public InteractionBase nowInteractionInfo;
        /// <summary>        /// 是否正在交互中        /// </summary>
        public bool isInteracting;
        /// <summary>        /// 射线检测的距离        /// </summary>
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

        /// <summary>        /// 挂到主角上，由主角时时交互        /// </summary>
        private void Start()
        {
            playerControl = gameObject.GetComponent<Control.PlayerControl>();
            isInteracting = false;
        }

        /// <summary>        /// 运行交互事件        /// </summary>
        /// <param name="interactionInfo">发生的交互事件</param>
        public void RunInteraction(InteractionBase interactionInfo)
        {
            isInteracting = true;   //设置正在交互
            if (interactionInfo == null) { 
                Debug.Log("交互对象空了");
            }
            //运行交互行为
            interactionInfo.InteractionBehavior();
        }

        /// <summary>
        /// 运行当前正在交互的交互事件
        /// </summary>
        public void RunInteraction()
        {
            if (nowInteractionInfo == null) return;
            RunInteraction(nowInteractionInfo);
        }

        /// <summary>   /// 表示停止交互，该系统重新开始工作    /// </summary>
        public void StopInteraction()
        {
            isInteracting = false;
        }

        /// <summary>        /// 开启交互        /// </summary>
        public void StartInteraction()
        {
            isInteracting = true;
        }

        public bool GetInteraction()
        {
            return isInteracting;
        }

        /// <summary>
        /// 添加一个交互事件，因为2D的交互变为触发式了
        /// </summary>
        public void AddInteraction(InteractionBase interaction)
        {
            nowInteractionInfo = interaction;
        }

        /// <summary>
        /// 清除当前的交互事件
        /// </summary>
        public void RemoveInteraction()
        {
            nowInteractionInfo = null;
        }

    }
}