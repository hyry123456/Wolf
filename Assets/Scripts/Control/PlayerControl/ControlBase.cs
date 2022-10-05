using UnityEngine;

namespace Control
{
    /// <summary>
    /// 角色控制器都要继承的基类，方便摄像机确定哪个是主角，
    /// 不提供什么方法，本质上只是用来获取对象用的
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

        protected bool isEnableInput;     //用来判断是否允许交互

        /// <summary>   /// 设置为允许输入    /// </summary>
        public void EnableInput()
        {
            isEnableInput = true;
        }
        /// <summary>   /// 设置为禁止输入    /// </summary>
        public void DisableInput()
        {
            isEnableInput = false;
        }

        /// <summary>       /// 获取中心位置      /// </summary>
        public abstract Vector3 GetPosition();


    }
}