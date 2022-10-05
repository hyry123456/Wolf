using UnityEngine;


namespace Control
{
    /// <summary> /// 2D������������ǣ�������ĸ�������һ���ٶȸ����  /// </summary>
    public class FollowPlayer2D : MonoBehaviour
    {
        Camera nowCamera;   //��ǰ�Ŀ��������
        /// <summary>  /// �������Ŀ��������ͷ���ĸ߶�    /// </summary>
        [SerializeField]
        float moveY = 5;
        /// <summary> /// ��������Ŀ�������ƫ��ֵ    /// </summary>
        float offsetDistance = 3;
        /// <summary>    /// ÿ��Ľӽ���������Ϊ0.5����ÿ����Сһ��    /// </summary>
        [SerializeField, Range(0.0001f, 1f)]
        float focusCentering = 0.5f;

        private void Start()
        {
            nowCamera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (ControlBase.Instance == null) return;
            Vector3 target = ControlBase.Instance.GetPosition() + Vector3.up * moveY;
            target.z = -10; //�����Ĭ�ϲ���
            Vector3 now = nowCamera.transform.position;
            float distance = Vector3.Distance(now, target);
            float t = 1.0f - focusCentering * Time.unscaledDeltaTime;
            
            if(distance > offsetDistance)
            {
                t = Mathf.Min(t, offsetDistance / distance);
            }
            nowCamera.transform.position = Vector3.Lerp(now, target, t);
        }
    }
}