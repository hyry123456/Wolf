using Interaction;
using UnityEngine;

namespace Task
{

    public class Chapter0_Part0 : ChapterPart
    {
        Chapter chapter;
        public override void EnterTaskEvent(Chapter chapter, bool isLoaded)
        {
            if (!isLoaded)
            {
                this.chapter = chapter;
                Common.SustainCoroutine.Instance.AddCoroutine(ShowDialog);
            }

        }

        bool ShowDialog()
        {
            UIExtentControl.Instance.ShowBigDialog(chapter.GetDiglogText(0),
                () =>
                {
                    UIExtentControl.Instance.ShowSmallDialog(chapter.GetDiglogText(1), null);
                    GameObject interacte = Common.SceneObjectMap.Instance.FindControlObject("TempInteracte");
                    InteracteDelegate @delegate = interacte.AddComponent<InteracteDelegate>();

                    GameObject gameObject = Common.SceneObjectMap.Instance.FindControlObject("Gateway1");
                    Gateway gateway = gameObject.GetComponent<Gateway>();
                    gateway.enabled = true;

                    @delegate.nonReturnAndNonParam = () =>
                    {
                        AsynTaskControl.Instance.CheckChapter(0, new InteracteInfo
                        {
                            data = "0_0",
                        });
                    };
                });
            return true;
        }

        public override void ExitTaskEvent(Chapter chapter)
        {
        }

        public override bool IsCompleteTask(Chapter chapter, InteracteInfo info)
        {
            if(info.data == "0_0")
            {
                UIExtentControl.Instance.ShowSmallDialog("获得传送技能\n与传送门交互可进行传送",
                    null);
                return true;
            }
            return false;
        }

    }
}