using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        private Vector3 beginPos;
        private Text text;
        public DefferedRender.PostFXSetting fXSetting;

        private void Awake()
        {
            if(instance != null)
            {
                Debug.LogError("ÖØ¸´µÄUI");
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
            }
        }

        private void OnEnd()
        {
            gameObject.SetActive(false);
            Control.PlayerControl.Instance?.EnableInput();     //¿ªÆôÊäÈë
            Camera.main.transform.position = beginPos;
            Motor.FollowPlayer2D.Instance?.BeginFollow();     //Í£Ö¹ÉãÏñ»ú¸úËæÖ÷½Ç
            fXSetting.EndWave();
            if (endBehavior != null)
                endBehavior();
        }

        public void ShowDumbText(string strs, Common.INonReturnAndNonParam endBehavior)
        {
            gameObject.SetActive(true);
            this.endBehavior = endBehavior;
            Motor.FollowPlayer2D.Instance?.StopFollow();     //Í£Ö¹ÉãÏñ»ú¸úËæÖ÷½Ç
            beginPos = Camera.main.transform.position;
            Camera.main.transform.position = transform.position + -Vector3.forward * 10;
            Control.PlayerControl.Instance?.DisableInput();     //Í£Ö¹ÊäÈë
            fXSetting.BeginWave();

            strings = new List<string>(strs.Split('\n'));
            text.text = strings[0];
            strings.RemoveAt(0);
        }


    }
}