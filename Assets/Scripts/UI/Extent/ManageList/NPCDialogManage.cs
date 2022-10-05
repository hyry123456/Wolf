using UnityEngine;

namespace UI
{
    /// <summary>
    /// NPC的世界UI显示的统一调用类，这个类用来启动以及控制
    /// NPCDialogUI组件的显示以及关闭
    /// </summary>
    public class NPCDialogManage : UIUseBase
    {
        GameObject origin;

        protected override void Awake()
        {
            base.Awake();
            control.init += ShowSelf;
            origin = Resources.Load<GameObject>("UI/NPC_DialogUI");
        }

        /// <summary>  /// 创建一个世界空间的对话UI，显示在一个角色的头上 /// </summary>
        /// <param name="strs">需要显示的所有文本，文本用换行符分割</param>
        /// <param name="followObj">需要跟随的对象</param>
        /// <param name="upHeight">距离跟随对象的高度</param>
        public void ShowDialog(string strs, Transform followObj, float upHeight, 
            Common.INonReturnAndNonParam endBehavior) 
        {
            NPCDialogUI dialogUI = (NPCDialogUI)Common.SceneObjectPool.Instance.
                GetObject("NPC_DialogUI", origin, transform.position, Quaternion.identity);
            dialogUI.ShowDialog(strs, followObj, upHeight, endBehavior);
            dialogUI.transform.parent = transform;      //设置为子物体，不然显示不出来
        }

        /// <summary> /// 创建一个世界空间的对话UI，显示在一个角色的头上  /// </summary>
        /// <param name="strs">需要显示的所有文本，文本用换行符分割</param>
        /// <param name="postion">创建在的位置</param>
        public void ShowDialog(string strs, Vector3 postion, Common.INonReturnAndNonParam endBehavior)
        {
            NPCDialogUI dialogUI = (NPCDialogUI)Common.SceneObjectPool.Instance.
                GetObject("NPC_DialogUI", origin, transform.position, Quaternion.identity);
            dialogUI.ShowDialog(strs, postion, endBehavior);
            dialogUI.transform.parent = transform;      //设置为子物体，不然显示不出来
        }
    }
}