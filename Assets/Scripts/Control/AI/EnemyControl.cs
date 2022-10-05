using Common;
using UnityEngine;

namespace Control
{

    /// <summary>
    /// 一个简单的敌人AI
    /// </summary>
    public class EnemyControl : ObjectPoolBase
    {
        Transform player;
        public LayerMask shelterMask;

        private Info.EnemyInfo enemyInfo;
        private Motor.Enemy2DMotor motor;

        /// <summary>     /// 每一次射出子弹需要等待的时间  /// </summary>
        public float buttleRayTime = 1;
        /// <summary>    /// 当前的等待时间    /// </summary>
        private float nowTime;
        public GameObject originBullet;

        //角度无所谓，毕竟敌人是个球
        public override void InitializeObject(Vector3 positon, Quaternion quaternion)
        {
            base.InitializeObject(positon, quaternion);
            player = PlayerControl.Instance.transform;
            motor = GetComponent<Motor.Enemy2DMotor>();
            enemyInfo = GetComponent<Info.EnemyInfo>();
        }

        public override void InitializeObject(Vector3 positon, Vector3 lookAt)
        {
            base.InitializeObject(positon, lookAt);
            player = PlayerControl.Instance.transform;
            motor = GetComponent<Motor.Enemy2DMotor>();
            enemyInfo = GetComponent<Info.EnemyInfo>();
        }

        private void FixedUpdate()
        {
            //首先判断怪物是否能够看到主角
            Vector3 offset = player.position - transform.position;
            float sqrDistance = offset.sqrMagnitude;
            //使用平方计算减少运算时间
            if (sqrDistance > enemyInfo.seeDistance * enemyInfo.seeDistance)     //看不到
            {
                motor.Move(0);
                return;
            }
            //看得到主角，判断是否被遮挡
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, offset, enemyInfo.seeDistance, shelterMask);
            if(hit2D.collider != null)  //发生了碰撞
            {
                if(hit2D.distance * hit2D.distance < sqrDistance) //是遮挡碰撞
                {
                    if(hit2D.distance < 3)     //太近了，跳过遮挡物体
                    {
                        motor.DesireJump();
                        motor.Move(offset.x);   //移动向主角
                    }
                    else
                        motor.Move(offset.x);   //移动向主角
                    nowTime = 0;
                    return;
                }
            }

            //没有碰撞，准备攻击主角
            nowTime += Time.fixedDeltaTime;
            motor.Move(0);      //停止移动，不要跑远了
            if (nowTime > buttleRayTime)     //只有达到释放时间才会攻击主角
            {
                Bullet_Pooling bullet_Pooling =
                    (Bullet_Pooling)SceneObjectPool.Instance.GetObject("Pooling_Bullet",
                    originBullet, transform.position + offset.normalized,
                    player.position);
                bullet_Pooling.attackTargetTag = "Player";
                nowTime = 0;
            }
        }

        protected void OnEnable()
        {
            nowTime = 0;
        }
    }
}