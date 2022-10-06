using UnityEngine;

namespace Motor
{
    /// <summary>
    /// һ��Ľ�ɫ�˶��࣬��ɫֻ�м򵥵Ĳɼ�Ҫ�أ�û�л�����ڵ��ƶ�Ч����
    /// ����Ϊ�˸����ˣ�û�м��ٶ�
    /// </summary>
    public class HumanMotor : MoveBase
    {
        [SerializeField]
        Vector2 nowVerticle = Vector2.zero;
        [SerializeField]
        bool canClimb;

        Info.CharacterInfo info;

        /// <summary> /// ������Ծ�߶�  /// </summary>
        public float jumpHeight = 2f;
        /// <summary>    /// �����Ծ����    /// </summary>
        public int maxAirJumps = 2;

        int nowJumps = 0;

        /// <summary>  /// ���������б�нǣ������������������������ʱ��ת��Ϊ����  /// </summary>
        [Range(0, 90)]
        public float maxGroundAngle = 25f;
        private float minGroundDot = 0;

        private void Start()
        {
            info = GetComponent<Info.CharacterInfo>();
            minGroundDot = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        }

        private void FixedUpdate()
        {
            Vector3 pos = transform.position;
            pos.x += nowVerticle.x * Time.fixedDeltaTime;
            pos.y += nowVerticle.y * Time.fixedDeltaTime;
            transform.position = pos;

            if (canClimb)
            {
                nowVerticle.y = 0;
            }
            else
            {
                nowVerticle.y += Time.fixedDeltaTime * -1;
            }
            nowVerticle.x = 0;
            canClimb = false;
        }

        private void OnCollisionStay(Collision collision)
        {
            float minDot = minGroundDot;
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector2 normal = collision.GetContact(i).normal;
                float upDot = Vector2.Dot(Vector2.up, normal);
                if (upDot >= minDot)
                {
                    nowVerticle.y = 0;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.tag == "Climb")
            {
                canClimb = true;
            }
        }

        public override void Climb(float vertical)
        {
            if (canClimb)
            {
                nowVerticle.y += vertical * info.walkSpeed;
            }
        }


        public override void DesireJump()
        {
            if(nowJumps < maxAirJumps)
            {
                float jumpSpeed = Mathf.Sqrt(2f * -Physics.gravity.y * jumpHeight);
                nowVerticle.y += jumpSpeed;
                nowJumps = 0;
            }
        }

        public override void Move(float horizontal)
        {
            nowVerticle.x = horizontal * info.runSpeed;
        }

        public override bool OnGround()
        {
            if (nowVerticle.y < 0.1f || nowVerticle.y > 0.1f)
                return false;
            return true;
        }
    }
}
