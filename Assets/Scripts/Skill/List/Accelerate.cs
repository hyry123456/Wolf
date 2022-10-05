using UnityEngine;

namespace Skill
{
    /// <summary>  /// 加速技能，进行实现主角闪避效果  /// </summary>
    public class Accelerate : SkillBase
    {
        public Accelerate()
        {
            expendSP = 0;
            nowCoolTime = 0;
            coolTime = 3;
            skillName = "加速";
            skillType = SkillType.Dodge;
        }

        const float sustainTime = 1;
        float nowTime = 0, minForece = 10, maxForece = 5;
        Rigidbody2D rb;
        Camera camera;
        /// <summary>    /// 释放加速技能    /// </summary>
        public override void OnSkillRelease(SkillManage mana)
        {
            if (rb == null)
                rb = mana.GetComponent<Rigidbody2D>();
            camera = Camera.main;
            if (camera == null || rb == null) return;
            nowTime = 0;
            //将持续加速的方法入栈，进行加速
            Common.SustainCoroutine.Instance.AddCoroutine(SustainAccelate);
        }

        /// <summary>   /// 持续加速的方法，加速时调整fov   /// </summary>
        bool SustainAccelate()
        {
            nowTime += Time.deltaTime;
            if(nowTime < sustainTime)
            {
                float radio = 1.0f - Mathf.Abs(nowTime / sustainTime - 0.5f) / 0.5f;
                float trueForece = Mathf.Lerp(minForece, maxForece, radio);
                rb.AddForce(rb.transform.right * trueForece);
                return false;
            }
            return true;
        }
    }
}