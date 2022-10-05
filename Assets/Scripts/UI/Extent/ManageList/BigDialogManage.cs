using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BigDialogManage : UIDialogBase
    {
        /// <summary>
        /// 所有对象的老爸，控制显示与否
        /// </summary>
        [SerializeField]
        GameObject father;
        /// <summary>  /// 显示名称用的文本  /// </summary>
        [SerializeField]
        Text nameText;
        /// <summary>  /// 显示对话框用的文本  /// </summary>
        [SerializeField]
        Text diglogText;

        protected override void Awake()
        {
            base.Awake();
            father.SetActive(false);
        }

        protected override void CloseUI()
        {
            father.SetActive(false);
            if(endBehavior != null)
            {
                endBehavior();
                endBehavior = null;
            }
        }

        protected override Color GetTextColor()
        {
            return diglogText.color;
        }

        protected override string ReadyOneLineString(string str)
        {
            //名称和内容需要用‘|’分割
            string[] strs = str.Split('|');
            nameText.text = strs[0];
            return strs[1];
        }

        protected override void SetTextData(string text)
        {
            diglogText.text = text;
        }

        protected override void ShowUI()
        {
            father.SetActive(true);
        }

        public void ShowBigDialog(string strs, Common.INonReturnAndNonParam endBehavior)
        {
            ShowDialog(strs);
            this.endBehavior = endBehavior;
        }

        /// <summary>    /// 是否有输入回车键进入下一行    /// </summary>
        bool isDesireNext;

        protected override void OnWaitLine()
        {
            isDesireNext |= Input.GetKeyDown(KeyCode.Return);
        }

        protected override bool CheckWaitEnd()
        {
            if (isDesireNext)
            {
                isDesireNext = false;
                return true;
            }
            return false;
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(sb != null)
                    diglogText.text = sb.ToString();
                sb = null;
            }

        }
    }
}