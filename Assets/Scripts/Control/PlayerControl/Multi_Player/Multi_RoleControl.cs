using Common.ResetInput;
using Control;
using UnityEngine;


/// <summary>
/// ����1�����ɫ����
/// </summary>
public class Multi_RoleControl : ControlBase
{
    /// <summary>  /// ������Ҫ���п��Ƶ���Ϸ����  /// </summary>
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
    /// ʱʱˢ�µĿ������ԵĴ��λ��
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
    /// ����֡ˢ�µ����Լ���λ�ã�һЩû�б�Ҫ��֡����Ŀ�����������м���
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
    /// ������ǿ����λ�ã�Ҳ���������ǰ��
    /// </summary>
    public Vector3 GetLookatDir()
    {
        if (Camera.main == null) return Vector3.zero;
        return Camera.main.transform.forward;
    }

    /// <summary>
    /// ������������������
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
            //���·�
            Vector3 left = camera.ViewportToWorldPoint(Vector2.zero);
            //���Ϸ�
            Vector3 right = camera.ViewportToWorldPoint(Vector2.one);
            bool isOutSide = false;     //Ĭ�����ڷ�Χ�ڣ���Ҫ��С
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
