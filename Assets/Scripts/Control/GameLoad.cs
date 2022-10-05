using UnityEngine;

namespace Control
{
    /// <summary>    
    /// �����࣬��������ȫ���ļ��ط�����Ϊ�˱�֤���أ�ÿһ���ೡ������һ����
    /// �����ֻ�ǵ����࣬�����Ƿ��ظ������ǿ�������ʵ�ֵ�
    /// </summary>
    public class GameLoad : MonoBehaviour
    {
        private void Awake()
        {
            Common.SustainCoroutine sustain = Common.SustainCoroutine.Instance; //����Э��
            SceneChangeControl changeControl = SceneChangeControl.Instance;
            Task.AsynTaskControl.Instance.ReLoadTask();
            Application.targetFrameRate = -1;
        }
    }
}