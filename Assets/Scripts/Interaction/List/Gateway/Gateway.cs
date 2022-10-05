using UnityEngine;

namespace Interaction
{

    /// <summary>
    /// 传送门交互，用来将主角传送到某个位置，同时调用切换主角控制的方法
    /// </summary>
    public class Gateway : InteractionBase
    {
        /// <summary>    /// 要变成的主角    /// </summary>
        public Control.PlayerControl changeToPlayer;
        [SerializeField]
        /// <summary>   /// 需要调整的屏幕效果  /// </summary>
        public DefferedRender.PostFXSetting fXSetting;
        private float nowRadio, waitTime = 1.0f;


        public override void InteractionBehavior()
        {
            Control.ControlBase.Instance.DisableInput();
            nowRadio = 0;
            fXSetting.EnableRotate();
            Common.SustainCoroutine.Instance.AddCoroutine(ChangeMap, false);
        }

        /// <summary>
        /// 插入到协程中切换地图
        /// </summary>
        bool ChangeMap()
        {
            if(nowRadio < 1.0f)
            {
                fXSetting.SetColorFilter(Color.Lerp(Color.white, Color.black, Mathf.Clamp01(nowRadio)));
                fXSetting.SetRotateRadio(nowRadio);
                nowRadio += Time.deltaTime;
                if(nowRadio >= 1.0f)
                {
                    fXSetting.DisableRotate();
                    fXSetting.SetRotateRadio(0);
                    Control.PlayerControl.ChangeToPlayer(changeToPlayer);
                }
                return false;
            }
            else if(nowRadio < 1.0f + waitTime)
            {
                nowRadio += Time.deltaTime;
                return false;
            }
            else
            {
                fXSetting.SetColorFilter(Color.Lerp(Color.black, Color.white, Mathf.Clamp01(nowRadio - 1.0f - waitTime)));
                nowRadio += Time.deltaTime;
                if (nowRadio >= 2.0f + waitTime)
                {
                    fXSetting.SetColorFilter(Color.white);
                    Control.PlayerControl.Instance.EnableInput();
                    return true;
                }
                return false;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }
    }
}