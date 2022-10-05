using UnityEngine;

public class LetterControl : MonoBehaviour
{
    /// <summary> /// ��ת�ٶ�/// </summary>
    public float RotateSpeed = 50;
    /// <summary> /// ���ڵ�/// </summary>
    protected Transform Parent;
    /// <summary> /// �ƹ�ڵ�/// </summary>
    protected Transform Light;
    /// <summary> /// ��ʱ��/// </summary>
    protected int Timer = 0;
    /// <summary> /// ��¼�ƹ⿪�صı���/// </summary>
    protected bool IsLightActive = true;
    private void Start()
    {
        Parent = gameObject.transform.parent;
        Light = Parent.GetChild(1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")//�����ֲ���
        {
            other.gameObject.GetComponent<Info.CharacterInfo>()?.gainScore();//��÷���
            Parent.gameObject.SetActive(false);
        }
        else return;
    }
    private void Update()
    {
        Parent.Rotate(Vector3.up, Time.deltaTime * RotateSpeed);//תȦ
    }
    
    private void FixedUpdate()
    {
        LightSwitch();
    }
    /// <summary>///�жϵƹ⿪��/// </summary>
    private void LightSwitch()
    {
        Timer++;
        if (Timer % 100 == 0)
        {
            if (IsLightActive)
            {
                IsLightActive = false;
                Light.gameObject.SetActive(false);
            }
            else
            {
                IsLightActive = true;
                Light.gameObject.SetActive(true);
            }
        }
    }
}
