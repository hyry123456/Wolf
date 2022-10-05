using Common.ResetInput;
using UnityEngine;

namespace Control
{
    public class ChangeScenePlayer : ControlBase
    {
        /// <summary>  /// 所有需要进行控制的游戏对象  /// </summary>
        public GameObject[] needControls;
        private int nowIndex = 0;
        private Motor.MoveBase[] motors;
        private Interaction.InteractionControl[] interactionControls;
        private PlayerSkillControl[] skillControls;
        public float dieY = -100;

        private string horizontalName = "Horizontal";
        private string jumpName = "Jump";
        private string interacteName = "Interaction";

        [SerializeField]
        /// <summary>   /// 需要调整的屏幕效果  /// </summary>
        public DefferedRender.PostFXSetting fXSetting;
        private float nowRadio, waitTime = 1.0f;

        private void OnEnable()
        {
            if (needControls == null || needControls.Length == 0) return;
            motors = new Motor.MoveBase[needControls.Length];
            interactionControls = new Interaction.InteractionControl[needControls.Length];
            skillControls = new PlayerSkillControl[needControls.Length];
            nowIndex = 0;
            needControls[0].SetActive(true);
            motors[0] = needControls[0]?.GetComponent<Motor.MoveBase>();
            interactionControls[0] = needControls[0]?.GetComponent<Interaction.InteractionControl>();
            skillControls[0] = needControls[0]?.GetComponent<PlayerSkillControl>();

            for (int i = 1; i < needControls.Length; i++)
            {
                motors[i] = needControls[i]?.GetComponent<Motor.MoveBase>();
                interactionControls[i] = needControls[i]?.GetComponent<Interaction.InteractionControl>();
                skillControls[i] = needControls[i]?.GetComponent<PlayerSkillControl>();
                needControls[i].SetActive(false);
            }

            isEnableInput = true;
            instance = this;
        }

        /// <summary>
        /// 时时刷新的控制属性的存放位置
        /// </summary>
        private void Update()
        {
            if (!isEnableInput || skillControls == null) return;
            if (Input.GetMouseButtonDown(0))
            {
                skillControls[nowIndex]?.ReleaseChooseSkill();
            }

            if (needControls == null || needControls.Length == 0)
                return;

            for (int i = 0; i < needControls.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    if (needControls[i].activeSelf)  //正在运行就直接退出
                        return;
                    else
                    {
                        ControlBase.Instance.DisableInput();
                        nowRadio = 0;
                        fXSetting.EnableRotate();
                        targetIndex = i;
                        Common.SustainCoroutine.Instance.AddCoroutine(ChangeMap, false);
                        return;
                    }
                }
            }
        }

        int targetIndex;

        /// <summary>
        /// 插入到协程中切换地图
        /// </summary>
        bool ChangeMap()
        {
            if (nowRadio < 1.0f)
            {
                fXSetting.SetColorFilter(Color.Lerp(Color.white, Color.black, Mathf.Clamp01(nowRadio)));
                fXSetting.SetRotateRadio(nowRadio);
                nowRadio += Time.deltaTime;
                if (nowRadio >= 1.0f)
                {
                    fXSetting.DisableRotate();
                    fXSetting.SetRotateRadio(0);
                    needControls[nowIndex].SetActive(false);
                    nowIndex = targetIndex;
                    needControls[nowIndex].SetActive(true);
                }
                return false;
            }
            else if (nowRadio < 1.0f + waitTime)
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
                    ControlBase.Instance.EnableInput();
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// 物理帧刷新的属性计算位置，一些没有必要逐帧计算的可以在这里进行计算
        /// </summary>
        private void FixedUpdate()
        {
            if (!isEnableInput) return;
            float horizontal = MyInput.Instance.GetAsis(horizontalName);
            bool jump = MyInput.Instance.GetButtonDown(jumpName);
            bool esc = MyInput.Instance.GetButtonDown("ESC");
            bool interacte = MyInput.Instance.GetButtonDown(interacteName);


            if (motors != null)
            {
                motors[nowIndex]?.Move(horizontal);
                if (jump)
                    motors[nowIndex]?.DesireJump();
            }

            if (esc)
                UIExtentControl.Instance?.ShowOrClose();

            if (interacte && interactionControls != null)
            {
                interactionControls[nowIndex].RunInteraction();
            }

            if (transform.position.y < dieY)
                SceneChangeControl.Instance.ReloadActiveScene();
        }

        /// <summary>
        /// 获得主角看向的位置，也就是摄像机前方
        /// </summary>
        public Vector3 GetLookatDir()
        {
            if (Camera.main == null) return Vector3.zero;
            return Camera.main.transform.forward;
        }

        /// <summary>
        /// 获得摄像机的世界坐标
        /// </summary>
        public Vector3 GetCameraPos()
        {
            Camera camera = Camera.main;
            if (camera != null)
                return camera.transform.position;
            else return Vector3.zero;
        }

        public override Vector3 GetPosition()
        {
            return needControls[nowIndex].transform.position;
        }
    }
}