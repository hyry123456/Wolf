using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BigDialogManage : UIDialogBase
    {
        /// <summary>
        /// ���ж�����ϰ֣�������ʾ���
        /// </summary>
        [SerializeField]
        GameObject father;
        /// <summary>  /// ��ʾ�����õ��ı�  /// </summary>
        [SerializeField]
        Text nameText;
        /// <summary>  /// ��ʾ�Ի����õ��ı�  /// </summary>
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
            //���ƺ�������Ҫ�á�|���ָ�
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

        /// <summary>    /// �Ƿ�������س���������һ��    /// </summary>
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