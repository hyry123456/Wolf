using System.Collections;
using UnityEngine;

namespace Interaction
{
    /// <summary>    /// 交互信息类，用来存储交互内容    /// </summary>
    public struct InteracteInfo
    {
        public string data;
        public int id;
    }

    /// <summary>    /// 交互类基类，挂在物体上，用来执行简单的交互    /// </summary>
    public abstract class InteractionBase : MonoBehaviour
    {
        [HideInInspector]
        public InteractionType interactionType;
        /// <summary>        /// 需要注意，交互ID一般不是用于特别指定，而是在一些特殊情况下进行临时赋值用的
        /// 也就是说初始化不需要赋值该属性/// </summary>
        public int interactionID = 0;
        /// <summary>        /// 初始化类名称，顺便作为交互的提示信息        /// </summary>
        public string interactionName;
        GameObject origin;
        UI.InteracteUI interacteUI;

        public float upOffset = 1.3f;

        /// <summary>
        /// 初始化InteractionType还有interactionName
        /// </summary>
        protected virtual void OnEnable()
        {
            if(origin == null)
                origin = Resources.Load<GameObject>("UI/InteracteRemain");

            interacteUI = (UI.InteracteUI)Common.SceneObjectPool.
                Instance.GetObject("InteracteRemain", origin,
                transform.position + Vector3.up * upOffset, Quaternion.identity);
        }

        protected virtual void OnDisable()
        {
            interacteUI.CloseObject();
        }

        /// <summary>        /// 该交互行为需要干的事情        /// </summary>
        public abstract void InteractionBehavior();


        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            InteractionControl.Instance.AddInteraction(this);
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            InteractionControl.Instance.RemoveInteraction();
        }

    }
}