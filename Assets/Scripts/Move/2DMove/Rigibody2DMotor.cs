using UnityEngine;

namespace Motor
{
    [System.Serializable]
    /// <summary>  /// 参考3d版本实现的2D移动  /// </summary>
    public class Rigibody2DMotor : MoveBase
    {
        /// <summary>    /// velocity当前速度, desiredVelocity期望速度, 
        /// connectionVelocity连接物体的速度    /// </summary>
        Vector2 velocity, desiredVelocity, connectionVelocity;
        /// <summary>    /// 加速度，移动加速度以及空气加速度    /// </summary>
        public float groundAcceleration = 10f, airAcceleration = 5;

        Rigidbody2D body2D;
        /// <summary>  /// 连接的物体，当地面在移动时保证刚体能够一同移动 /// </summary>
        GameObject connectObj, preConnectObj;
        /// <summary> /// 用来判断是否打算跳跃  /// </summary>
        private bool desiredJump = false;

        /// <summary> /// 单次跳跃高度  /// </summary>
        public float jumpHeight = 2f;
        /// <summary>    /// 最大跳跃次数    /// </summary>
        public int maxAirJumps = 2;
        /// <summary>   /// 在空中移动的加速度 /// </summary>
        private int airJumps = 0;

        [SerializeField]
        /// <summary>    /// 是否在地面上    /// </summary>
        private bool onGround = false;

        /// <summary>  /// 最大地面的倾斜夹角，这个数组由外界调整，在运行时会转化为弧度  /// </summary>
        [Range(0, 90)]
        public float maxGroundAngle = 25f;
        private float minGroundDot = 0;

        /// <summary>
        /// 贴地检测用的距离
        /// </summary>
        [SerializeField]
        float probeDistance = 1;
        /// <summary>
        /// 贴地检测的层
        /// </summary>
        [SerializeField]
        LayerMask probeMask = -1;


        /// <summary> /// 接触面的法线，这个法线是平均法线，用来确定移动面的方向以及跳跃的方向 /// </summary>
        Vector2 contactNormal;

        /// <summary>   /// 角色信息，用来确定移动速度   /// </summary>
        Info.CharacterInfo characterInfo;
        /// <summary>  /// 接触面的世界坐标，用来判断接触面的移动距离，等值调动物体移动 /// </summary>
        Vector3 connectionWorldPostion;
        [SerializeField]
        private bool isClimb;

        /// <summary>
        /// 用来确定此时离开地面的时间(stepSinceLastGround)，在地面时会变为0，
        /// 不在时会逐物理帧刷新
        /// </summary>
        int stepSinceLastGround = 0;
        /// <summary>    /// 用来确定跳跃的时间，当跳跃时会归零，在物理帧时逐帧增加    /// </summary>
        int stepSinceLastJump = 0;



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
            CheckTransing();
            AdjustVelocity();
            if (desiredJump)
            {
                Jump();
                desiredJump = false;
            }
            Rotate();

            velocity = Vector2.ClampMagnitude(velocity, 40f);

            body2D.velocity = velocity;
            ClearState();

        }


        Vector3 targetPos;      //传送到的目标点
        Vector3 direct;         //移动方向
        float maxSpeed = -1;         //最大速度

        /// <summary>
        /// 传送到特定点，在传送过程中需要停止其他力，只剩下向目标点的速度
        /// </summary>
        /// <param name="postion">目标位置</param>
        /// <param name="speed">速度</param>
        public void TransferToPosition(Transform postion, float speed)
        {
            //不允许反复传送
            if (maxSpeed > 0)
            {
                maxSpeed = -1;  //停止钩锁
                //body.useGravity = true;
                HookRopeManage.Instance.CloseHookRope();
                return;
            }
            if (postion == null) return;
            direct = (postion.position - transform.position).normalized;
            Vector2 nowDir = body2D.velocity.normalized;
            body2D.velocity = Mathf.Clamp01(Vector3.Dot(nowDir, direct)) * body2D.velocity;

            HookRopeManage.Instance.LinkHookRope(postion.position, transform);

            maxSpeed = speed;
            targetPos = postion.position;
        }

        /// <summary>        /// 进行传送        /// </summary>
        /// <returns>是否在传送中</returns>
        private bool CheckTransing()
        {
            if (maxSpeed < 0) return false;

            Vector2 dir = (targetPos - transform.position).normalized;
            if (Vector3.Dot(dir, direct) < 0.3)
            {
                maxSpeed = -1;
                //body.useGravity = true;
                HookRopeManage.Instance.CloseHookRope();
                return false;
            }

            velocity += dir * maxSpeed;
            return true;
        }
        void UpdateState()
        {
            velocity = body2D.velocity;
            stepSinceLastGround ++;
            stepSinceLastJump++;
            //当不在地面时执行贴近地面方法
            if (onGround/*在地上*/ || SnapToGround())
            {
                airJumps = 0;
                stepSinceLastGround = 0;
                contactNormal.Normalize();
            }
            else
                contactNormal = Vector2.up;

            if (connectObj && connectObj.tag == "CheckMove")
            {
                UpdateConnectionState();
            }

            //if(!isClimb)
            //{
            //    body2D.simulated = true;
            //}
            //else
            //{
            //    body2D.simulated = false;
            //    Vector2 speed = body2D.velocity; speed.y = 0;
            //    body2D.velocity = speed;
            //}
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
            else if (airJumps < maxAirJumps)    //在空中，要小于最大跳跃次数
            {
                //如果不在地上也不在斜面，并且可以在空中跳跃
                jumpDirction = Vector3.up;
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
            //跳跃计数
            airJumps++;
            stepSinceLastJump = 0;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Climb")
                isClimb = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Climb")
                isClimb = false;
        }

        void EvaluateCollision(Collision2D collision)
        {
            onGround = false;
            contactNormal = connectionVelocity = Vector2.zero;
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
            if (isClimb)
                velocity.y += -Physics.gravity.y * Time.fixedDeltaTime;
        }

        /// <summary>  /// 清除数据，把一些数据归为初始化  /// </summary>
        void ClearState()
        {
            preConnectObj = connectObj;
            connectObj = null;
        }

        /// <summary>    /// 确定该方向投影到该平面上的方向值，结果进行过标准化    /// </summary>
        Vector2 ProjectDirectionOnPlane(Vector2 direction, Vector2 normal)
        {
            return (direction - normal * Vector2.Dot(direction, normal)).normalized;
        }


        /// <summary>    /// 旋转模型     /// </summary>
        void Rotate()
        {
            //旋转朝向的目标点
            if(velocity.x != 0)
            {
                Vector3 rotateTarget = transform.position;
                rotateTarget += (velocity.x > 0) ? Vector3.forward : -Vector3.forward;
                transform.LookAt(rotateTarget);
            }

        }

        public override void Move(float horizontal)
        {
            desiredVelocity = Vector2.right * horizontal;
            desiredVelocity = desiredVelocity * characterInfo.runSpeed;
        }

        public override void DesireJump()
        {
            desiredJump = true;
        }

        public override bool OnGround()
        {
            return onGround;
        }

        public override void Climb(float vertical)
        {
            if (isClimb)
            {
                Vector3 pos = transform.position;
                pos.y += vertical * Time.fixedDeltaTime * characterInfo.walkSpeed;
                transform.position = pos;
            }
        }

        /// <summary>
        /// 用于贴近地面用的方法，减少移动时会飞出去的效果
        /// </summary>
        /// <returns>用来配合一些地面检测使用，因此有返回值</returns>
        bool SnapToGround()
        {
            //贴地行为只进行一次，同时用跳跃时间避免跳跃时贴地
            if (stepSinceLastGround > 1 || stepSinceLastJump <= 2)
            {
                return false;
            }
            float speed = velocity.magnitude;


            RaycastHit2D hit;
            hit = Physics2D.Raycast(body2D.position, Vector2.down, probeDistance, probeMask);
            if (hit.collider == null)
                return false;

            float upDot = Vector2.Dot(Vector2.up, hit.normal);
            //如果射中的面不能作为可以站立的面，就不进行贴近
            if (upDot < minGroundDot)
                return false;


            contactNormal = hit.normal;

            //确定速度在法线上的大小
            float dot = Vector2.Dot(velocity, hit.normal);
            //保证只有速度朝上时才会往下压，不会减少下落速度
            if (dot > 0)
            {
                //根据速度的大小往平面上压
                velocity = (velocity - hit.normal * dot).normalized * speed;
            }
            else
            {
                velocity = (velocity + hit.normal * dot).normalized * speed;
            }
            return true;
        }

    }
}