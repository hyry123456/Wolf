using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary> /// С�ĶԻ��� /// </summary>
    public class SmallDialogManage : UIDialogBase
    {
        [SerializeField]
        Text smallDialog;

        protected override void Awake()
        {
            base.Awake();
            smallDialog.gameObject.SetActive(false);
        }

        protected override void CloseUI()
        {
            smallDialog.gameObject.SetActive(false);
            if(endBehavior != null)
            {
                endBehavior();
                endBehavior = null;
            }
        }
        protected override Color GetTextColor()
        {
            return smallDialog.color;
        }
        protected override string ReadyOneLineString(string str)
        {
            return str;
        }
        protected override void SetTextData(string text)
        {
            smallDialog.text = text;
        }
        protected override void ShowUI()
        {
            smallDialog.gameObject.SetActive(true);
        }

        public void ShowSmallDialog(string strs, Common.INonReturnAndNonParam endBehavior)
        {
            ShowDialog(strs);
            this.endBehavior = endBehavior;
        }

        /// <summary>    /// ûһ��ͣ���ĵȴ�ʱ��     /// </summary>
        float waitOneLineMax = 2f;
        float nowTime;  //��ǰ�ĵȴ�ʱ��

        protected override void OnWaitLine()
        {
            nowTime += Time.deltaTime;
        }

        protected override bool CheckWaitEnd()
        {
            if(nowTime >= waitOneLineMax)
            {
                nowTime = 0;
                return true;
            }
            return false;
        }
    }
}