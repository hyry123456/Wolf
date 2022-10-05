using UnityEngine;
using Common;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 池化的交互显示UI，用来标识这个是一个可交互的物体
    /// </summary>
    public class InteracteUI : ObjectPoolBase
    {
        ///// <summary>    /// 显示用的图片     /// </summary>
        //Image showImage;
        /// <summary>   /// 用来判断交互的颜色是上升还是下降   /// </summary>
        bool isUp;
        /// <summary>  /// 颜色的上升速度    /// </summary>
        public float upSpeed = 1;
        public float upSize = 0.3f;
        Vector2 begin, end;
        float nowRadio;

        public override void InitializeObject(Vector3 positon, Quaternion quaternion)
        {
            base.InitializeObject(positon, quaternion);

            begin = positon; begin += Vector2.up * upSize;
            end = positon; end -= Vector2.up * upSize;
            nowRadio = 0.5f; isUp = true;
        }

        public override void InitializeObject(Vector3 positon, Vector3 lookAt)
        {
            base.InitializeObject(positon, lookAt);

            begin = positon; begin += Vector2.up * upSize;
            end = positon; end -= Vector2.up * upSize;
            nowRadio = 0.5f; isUp = true;
        }

        public void Update()
        {
            nowRadio += (isUp)? Time.deltaTime * upSpeed : -upSpeed * Time.deltaTime;
            if(nowRadio < 0)
            {
                nowRadio = 0; isUp = true;
            }
            else if(nowRadio > 1)
            {
                nowRadio = 1; isUp = false;
            }
            transform.position = Vector2.Lerp(begin, end, nowRadio);
        }

    }
}