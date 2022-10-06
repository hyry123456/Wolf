using UnityEngine;

namespace Task
{
    /// <summary>
    /// ��һ�£�����������η����Ĺ��µĹ��±������Լ����ǵ�Ŀ��֮���
    /// </summary>
    public class Chapter0 : AsynChapterBase
    {
        public Chapter0()
        {
            chapterName = "���� ����֮��";
            chapterTitle = "����";
            taskPartCount = 1;
            chapterID = 0;
            chapterSavePath = Application.streamingAssetsPath + "/Task/Chapter/0.task";
            targetPart += "Chapter0_Part";
            runtimeScene = "MainScene";
        }

        /// <summary>  /// ������½ڵ�һ�£������˾�˵���ǿ�ʼ��Ϸ   /// </summary>
        public override void CheckAndLoadChapter()
        {
            AsynTaskControl.Instance.AddChapter(this);
        }

        public override void BeginChapter()
        {
            base.BeginChapter();
            Common.SustainCoroutine.Instance.AddCoroutine(SetGatewayDisable);
        }

        public override void CompleteChapter()
        {
            Common.SustainCoroutine.Instance.AddCoroutine(SetGatewayEnable);
        }

        bool SetGatewayEnable()
        {
            GameObject gameObject = Common.SceneObjectMap.Instance.FindControlObject("Gateway1");
            Interaction.Gateway gateway = gameObject.GetComponent<Interaction.Gateway>();
            gateway.enabled = true;
            return true;
        }

        bool SetGatewayDisable()
        {
            GameObject gameObject = Common.SceneObjectMap.Instance.FindControlObject("Gateway1");
            Interaction.Gateway gateway = gameObject.GetComponent<Interaction.Gateway>();
            gateway.enabled = false;
            return true;
        }

        public override void ExitChapter()
        {
        }
    }
}