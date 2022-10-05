using UnityEngine;


namespace Motor
{
    /// <summary> /// 2D摄像机跟随主角，摄像机的跟随是以一个速度跟随的  /// </summary>
    public class FollowPlayer2D : MonoBehaviour
    {
        private static FollowPlayer2D instance;
        public static FollowPlayer2D Instance => instance;

        Camera nowCamera;   //当前的控制摄像机
        /// <summary>  /// 摄像机的目标点距主角头部的高度    /// </summary>
        [SerializeField]
        float moveY = 5;
        /// <summary> /// 摄像机相对目标点最大的偏移值    /// </summary>
        float offsetDistance = 3;
        /// <summary>    /// 每秒的接近比例，设为0.5就是每秒缩小一半    /// </summary>
        [SerializeField, Range(0.0001f, 1f)]
        float focusCentering = 0.5f;


        bool isFollow = false;
        public void BeginFollow()
        {
            isFollow = true;
        }
        public void StopFollow()
        {
            isFollow = false;
        }

        private void Awake()
        {
            if (instance != null)
                Debug.LogError("多个摄像机");
            instance = this;
            isFollow = true;
        }

        private void Start()
        {
            nowCamera = GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            if (Control.ControlBase.Instance == null || !isFollow) return;
            Vector3 target = Control.ControlBase.Instance.GetPosition() + Vector3.up * moveY;
            target.z = -10; //摄像机默认参数
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