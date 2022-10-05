using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Motor
{
    public class Enemy2DMotor : MonoBehaviour
    {        /// <summary>    /// velocity当前速度, desiredVelocity期望速度, 
             /// connectionVelocity连接物体的速度    /// </summary>
        Vector2 velocity, desiredVelocity, connectionVelocity;
        /// <summary>    /// 加速度，移动加速度以及空气加速度    /// </summary>
        public float groundAcceleration = 10f, airAcceleration = 5;

        Rigidbody2D body2D;
        /// <summary>  /// 连接的物体，当地面在移动时保证刚体能够一同移动 /// </summary>
        GameObject connectObj, preConnectObj;

        /// <summary> /// 单次跳跃高度  /// </summary>
        public float jumpHeight = 2f;
        /// <summary>    /// 最大跳跃次数    /// </summary>
        public int maxAirJumps = 2;
        /// <summary>    /// 是否在地面上    /// </summary>
        private bool onGround = false;

        /// <summary>  /// 最大地面的倾斜夹角，这个数组由外界调整，在运行时会转化为弧度  /// </summary>
        [Range(0, 90)]
        public float maxGroundAngle = 25f;
        private float minGroundDot = 0;

        /// <summary> /// 接触面的法线，这个法线是平均法线，用来确定移动面的方向以及跳跃的方向 /// </summary>
        Vector2 contactNormal;

        /// <summary>   /// 角色信息，用来确定移动速度   /// </summary>
        Info.CharacterInfo characterInfo;
        /// <summary>  /// 接触面的世界坐标，用来判断接触面的移动距离，等值调动物体移动 /// </summary>
        Vector3 connectionWorldPostion;
        /// <summary>  /// 判断是否想要跳跃，跳跃指令不是立刻执行的   /// </summary>
        bool desiredJump;

        void Awake()
        {
            velocity = Vector3.zero;
            body2D = GetComponent<Rigidbody2D>();
            characterInfo = GetComponent<Info.CharacterInfo>();
            minGroundDot = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
            if (characterInfo == null) Debug.LogError("角色信息为空");
        }


        private void FixedUpdate()
        {
            //更新数据，用来对这一个物理帧的数据进行更新之类的
            UpdateState();

            //确定在空中还是在地面
            AdjustVelocity();

            if (desiredJump)
            {
                Jump();
                desiredJump = false;
            }
            Rotate();

            body2D.velocity = velocity;
            ClearState();
        }

        /// <summary>   /// 输入的移动值，由于是2D，因此只有水平移动   /// </summary>
        /// <param name="horizontal">水平移动值</param>
        public void Move(float horizontal)
        {
            horizontal = Mathf.Clamp(horizontal, -1.0f, 1.0f);
            desiredVelocity = Vector2.right * horizontal * characterInfo.runSpeed;
        }

        void UpdateState()
        {
            velocity = body2D.velocity;
            //当不在地面时执行贴近地面方法
            if (onGround/*在地上*/)
            {
                contactNormal.Normalize();
            }
            else
                contactNormal = Vector3.up;


            if (connectObj && connectObj.tag == "CheckMove")
            {
                UpdateConnectionState();
            }
        }

        void UpdateConnectionState()
        {
            //只有物体相同，才有必要计算
            if (connectObj == preConnectObj)
            {
                Vector3 connectionMovment =
                    connectObj.transform.position - connectionWorldPostion;
                connectionVelocity = connectionMovment / Time.deltaTime;
            }
            connectionWorldPostion = connectObj.transform.position;
        }

        void Jump()
        {
            Vector2 jumpDirction;
            //确定跳跃方向
            if (onGround)       //在地面上，比如在平面或者在一个不是很斜的斜面上
            {
                //在地上，直接根据接触方向
                jumpDirction = contactNormal;
            }
            //不能跳，退出
            else return;

            //根据重力的大小来确定移动速度，保证跳跃高度为我们的目标高度
            float jumpSpeed = Mathf.Sqrt(2f * -Physics.gravity.y * jumpHeight);
            //确定两者点乘后的大小，判断当前速度与跳跃的方向的关系
            float aligneSpeed = Vector2.Dot(velocity, jumpDirction);
            if (aligneSpeed > 0)    //两者在不同方向
            {
                //缩小跳跃高度，防止连续跳跃叠加得到很高的跳跃数值
                jumpSpeed = Mathf.Max(jumpSpeed - aligneSpeed, 0);
            }
            //添加最后的跳跃速度
            velocity += jumpDirction * jumpSpeed;
        }

        public void DesireJump()
        {
            desiredJump = true;
        }


        /// <summary>  /// 接触退出时加载一次接触面法线  /// </summary>
        private void OnCollisionExit2D(Collision2D collision)
        {
            EvaluateCollision(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            EvaluateCollision(collision);
        }

        void EvaluateCollision(Collision2D collision)
        {
            float minDot = minGroundDot;
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector2 normal = collision.GetContact(i).normal;
                float upDot = Vector2.Dot(Vector2.up, normal);
                if (upDot >= minDot)
                {
                    onGround = true;
                    //保证如果有多个接触面时能够正确的获取法线
                    contactNormal += normal;
                    connectObj = collision.gameObject;
                }
                //陡峭面移动控制，但是避免彻底的垂直面
                else if (upDot > -0.01f)
                {
                    connectObj = collision.gameObject;
                }

            }
        }

        /// <summary>
        /// 调整移动方向，用来保证移动的方向是沿着平面的
        /// </summary>
        void AdjustVelocity()
        {
            Vector2 xAixs = ProjectDirectionOnPlane(Vector2.right, contactNormal);
            Vector2 yAxis = ProjectDirectionOnPlane(Vector2.up, contactNormal);

            Vector2 relativeVelocity = velocity - connectionVelocity;
            //确定实际上在这个平面上的X移动值
            float currentX = Vector2.Dot(relativeVelocity, xAixs);
            //确定实际上在这个平面上的Z移动值
            float currentY = Vector2.Dot(relativeVelocity, yAxis);

            //确定加速度大小
            float acceleration = onGround ? groundAcceleration : airAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;


            ////确定根据期望得到的移动值
            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newY = Mathf.MoveTowards(currentY, desiredVelocity.y, maxSpeedChange);
            velocity += xAixs * (newX - currentX) + yAxis * (newY - currentY);

            //if (!onGround)      //在空中时调整移动速度，避免跳不过障碍
            //{
            //    velocity.x += Mathf.Clamp(velocity.x, -1.0f, 1.0f) * 0.1f;
            //}
        }

        /// <summary>  /// 清除数据，把一些数据归为初始化  /// </summary>
        void ClearState()
        {
            onGround = false;
            contactNormal = connectionVelocity = Vector2.zero;
            preConnectObj = connectObj;
            connectObj = null;
        }

        /// <summary>    /// 确定该方向投影到该平面上的方向值，结果进行过标准化    /// </summary>
        Vector2 ProjectDirectionOnPlane(Vector2 direction, Vector2 normal)
        {
            return (direction - normal * Vector2.Dot(direction, normal)).normalized;
        }

        /// <summary>   /// 旋转模型     /// </summary>
        void Rotate()
        {
            //旋转朝向的目标点
            if (velocity.x != 0)
            {
                Vector3 rotateTarget = transform.position;
                rotateTarget += (velocity.x > 0) ? Vector3.forward : -Vector3.forward;
                transform.LookAt(rotateTarget);
            }

        }

    }
}