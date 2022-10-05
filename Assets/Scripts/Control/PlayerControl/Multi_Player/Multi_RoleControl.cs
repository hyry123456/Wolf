using Common.ResetInput;
using Control;
using UnityEngine;


/// <summary>
/// 需求1，多角色控制
/// </summary>
public class Multi_RoleControl : ControlBase
{
    /// <summary>  /// 所有需要进行控制的游戏对象  /// </summary>
    public GameObject[] needControls;
    private Motor.MoveBase[] motors;
    private Interaction.InteractionControl[] interactionControls;
    private PlayerSkillControl[] skillControls;
    public float dieY = -100;

    public string verticalName = "Vertical";
    public string horizontalName = "Horizontal";
    public string jumpName = "Jump";
    public string setName = "Setting";
    public string begName = "Bag";
    public string interacteName = "Interaction";

    private void OnEnable()
    {
        if (needControls == null || needControls.Length == 0) return;
        motors = new Motor.MoveBase[needControls.Length];
        interactionControls = new Interaction.InteractionControl[needControls.Length];
        skillControls = new PlayerSkillControl[needControls.Length];
        for (int i = 0; i < needControls.Length; i++)
        {
            motors[i] = needControls[i]?.GetComponent<Motor.MoveBase>();
            interactionControls[i] = needControls[i]?.GetComponent<Interaction.InteractionControl>();
            skillControls[i] = needControls[i]?.GetComponent<PlayerSkillControl>();
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
            for(int i=0; i<skillControls.Length; i++)
            {
                skillControls[i]?.ReleaseChooseSkill();
            }
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

        if(motors != null)
        {
            for(int i=0; i<motors.Length; i++)
            {
                motors[i]?.Move(horizontal);
                if (jump)
                    motors[i]?.DesireJump();
            }
        }



        if (esc)
            UIExtentControl.Instance?.ShowOrClose();

        if (interacte && interactionControls != null)
        {
            for(int i=0; i<interactionControls.Length; i++)
            {
                interactionControls[i].RunInteraction();
            }
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
        Camera camera = Camera.main;
        if (camera != null)
        {
            //左下方
            Vector3 left = camera.ViewportToWorldPoint(Vector2.zero);
            //右上方
            Vector3 right = camera.ViewportToWorldPoint(Vector2.one);
            bool isOutSide = false;     //默认是在范围内，需要缩小
            float disX = right.x - left.x, disY = right.y - left.y;
            for (int i = 0; i < needControls.Length; i++)
            {
                Vector3 vector3 = needControls[i].transform.position;
                float radioX = (vector3.x - left.x) / disX;
                float radioY = (vector3.y - left.y) / disX;
                if (radioX < 0.2f || radioX > 0.8f || radioY < 0.1f || radioY > 0.9f)
                {
                    isOutSide = true;
                    break;
                }
            }
            if (isOutSide)
            {
                camera.orthographicSize += Time.deltaTime;
            }
            else
            {
                camera.orthographicSize = Mathf.Max(camera.orthographicSize - Time.deltaTime, 5);
            }
        }

        if (needControls == null)
            return transform.position;
        Vector3 all = Vector3.zero;
        for(int i=0; i<needControls.Length; i++)
        {
            all += needControls[i].transform.position;
        }
        return all / needControls.Length;
    }
}
