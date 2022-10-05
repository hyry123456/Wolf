using UnityEngine;

namespace Motor
{
    /// <summary>
    /// 一般的角色运动类，角色只有简单的采集要素，没有花里胡哨的移动效果，
    /// 而且为了更像人，没有加速度
    /// </summary>
    public class HumanMotor : MoveBase
    {
        Rigidbody2D body2D;
        Info.CharacterInfo characterInfo;

        private void OnEnable()
        {
            body2D = GetComponent<Rigidbody2D>();
            characterInfo = GetComponent<Info.CharacterInfo>();
        }

        public override void DesireJump()
        {
        }

        public override void Move(float horizontal)
        {
            //得出水平移动值
            Vector2 horiVelocity = Vector2.right * horizontal * characterInfo.runSpeed;
            horiVelocity.y = body2D.velocity.y; //保留坠落速度
            body2D.velocity = horiVelocity;
        }

        public override bool OnGround()
        {
            return true;
        }
    }
}