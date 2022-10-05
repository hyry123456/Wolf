using UnityEngine;

namespace Interaction
{

    /// <summary>
    /// �����Ž��������������Ǵ��͵�ĳ��λ�ã�ͬʱ�����л����ǿ��Ƶķ���
    /// </summary>
    public class Gateway : InteractionBase
    {
        /// <summary>    /// Ҫ��ɵ�����    /// </summary>
        public Control.PlayerControl changeToPlayer;
        [SerializeField]
        /// <summary>   /// ��Ҫ��������ĻЧ��  /// </summary>
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
        /// ���뵽Э�����л���ͼ
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