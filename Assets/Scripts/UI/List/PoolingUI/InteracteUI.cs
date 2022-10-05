using UnityEngine;
using Common;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// �ػ��Ľ�����ʾUI��������ʶ�����һ���ɽ���������
    /// </summary>
    public class InteracteUI : ObjectPoolBase
    {
        ///// <summary>    /// ��ʾ�õ�ͼƬ     /// </summary>
        //Image showImage;
        /// <summary>   /// �����жϽ�������ɫ�����������½�   /// </summary>
        bool isUp;
        /// <summary>  /// ��ɫ�������ٶ�    /// </summary>
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