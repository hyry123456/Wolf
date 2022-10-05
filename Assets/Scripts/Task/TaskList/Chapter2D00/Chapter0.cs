using UnityEngine;

namespace Task
{
    /// <summary>
    /// 第一章，用来介绍这次发生的故事的故事背景，以及主角的目标之类的
    /// </summary>
    public class Chapter0 : AsynChapterBase
    {
        public Chapter0()
        {
            chapterName = "序章 马云之死";
            chapterTitle = "好死";
            taskPartCount = 1;
            chapterID = 0;
            chapterSavePath = Application.streamingAssetsPath + "/Task/Chapter/0.task";
            targetPart += "Chapter0_Part";
            runtimeScene = "MainScene";
        }

        /// <summary>  /// 这个是章节第一章，调用了就说明是开始游戏   /// </summary>
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