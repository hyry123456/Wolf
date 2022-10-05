using UnityEngine;

namespace Motor
{
    /// <summary>
    /// һ��Ľ�ɫ�˶��࣬��ɫֻ�м򵥵Ĳɼ�Ҫ�أ�û�л�����ڵ��ƶ�Ч����
    /// ����Ϊ�˸����ˣ�û�м��ٶ�
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
            //�ó�ˮƽ�ƶ�ֵ
            Vector2 horiVelocity = Vector2.right * horizontal * characterInfo.runSpeed;
            horiVelocity.y = body2D.velocity.y; //����׹���ٶ�
            body2D.velocity = horiVelocity;
        }

        public override bool OnGround()
        {
            return true;
        }
    }
}