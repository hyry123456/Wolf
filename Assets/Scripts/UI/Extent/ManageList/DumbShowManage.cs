using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

namespace UI
{
    public class DumbShowManage : MonoBehaviour
    {
        private static DumbShowManage instance;
        public static DumbShowManage Instance
        {
            get
            {
                return instance;
            }
        }

        private Common.INonReturnAndNonParam endBehavior;

        private List<string> strings;
        /// <summary>/// 所有的委托/// </summary>
        List<INonReturnAndNonParam> funtions;
        private Vector3 beginPos;
        private Text text;
        public DefferedRender.PostFXSetting fXSetting;

        private void Awake()
        {
            if(instance != null)
            {
                Debug.LogError("重复的UI");
            }
            instance = this;
            text = GetComponent<Text>();
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private void Update()
        {
            if(strings == null)
            {
                OnEnd();
                return;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(strings.Count == 0)
                {
                    OnEnd();
                    return;
                }
                text.text = strings[0];
                strings.RemoveAt(0);
                if (funtions[0] != null)
                {
                    funtions[0]();
                }
                funtions.RemoveAt(0);
            }
        }

        private void OnEnd()
        {
            gameObject.SetActive(false);
            Control.PlayerControl.Instance?.EnableInput();     //开启输入
            Camera.main.transform.position = beginPos;
            Motor.FollowPlayer2D.Instance?.BeginFollow();     //停止摄像机跟随主角
            fXSetting.EndWave();
            if (endBehavior != null)
                endBehavior();
        }

        public void ShowDumbText(string strs, Common.INonReturnAndNonParam endBehavior,
            INonReturnAndNonParam[] funtions)
        {
            this.funtions = new List<INonReturnAndNonParam>(funtions);
            gameObject.SetActive(true);
            this.endBehavior = endBehavior;
            Motor.FollowPlayer2D.Instance?.StopFollow();     //停止摄像机跟随主角
            beginPos = Camera.main.transform.position;
            Camera.main.transform.position = transform.position + -Vector3.forward * 10;
            Control.PlayerControl.Instance?.DisableInput();     //停止输入
            fXSetting.BeginWave();

            strings = new List<string>(strs.Split('\n'));
            text.text = strings[0];
            strings.RemoveAt(0);
        }


    }
}