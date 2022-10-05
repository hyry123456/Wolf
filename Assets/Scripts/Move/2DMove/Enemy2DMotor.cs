using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Motor
{
    public class Enemy2DMotor : MonoBehaviour
    {        /// <summary>    /// velocity��ǰ�ٶ�, desiredVelocity�����ٶ�, 
             /// connectionVelocity����������ٶ�    /// </summary>
        Vector2 velocity, desiredVelocity, connectionVelocity;
        /// <summary>    /// ���ٶȣ��ƶ����ٶ��Լ��������ٶ�    /// </summary>
        public float groundAcceleration = 10f, airAcceleration = 5;

        Rigidbody2D body2D;
        /// <summary>  /// ���ӵ����壬���������ƶ�ʱ��֤�����ܹ�һͬ�ƶ� /// </summary>
        GameObject connectObj, preConnectObj;

        /// <summary> /// ������Ծ�߶�  /// </summary>
        public float jumpHeight = 2f;
        /// <summary>    /// �����Ծ����    /// </summary>
        public int maxAirJumps = 2;
        /// <summary>    /// �Ƿ��ڵ�����    /// </summary>
        private bool onGround = false;

        /// <summary>  /// ���������б�нǣ������������������������ʱ��ת��Ϊ����  /// </summary>
        [Range(0, 90)]
        public float maxGroundAngle = 25f;
        private float minGroundDot = 0;

        /// <summary> /// �Ӵ���ķ��ߣ����������ƽ�����ߣ�����ȷ���ƶ���ķ����Լ���Ծ�ķ��� /// </summary>
        Vector2 contactNormal;

        /// <summary>   /// ��ɫ��Ϣ������ȷ���ƶ��ٶ�   /// </summary>
        Info.CharacterInfo characterInfo;
        /// <summary>  /// �Ӵ�����������꣬�����жϽӴ�����ƶ����룬��ֵ���������ƶ� /// </summary>
        Vector3 connectionWorldPostion;
        /// <summary>  /// �ж��Ƿ���Ҫ��Ծ����Ծָ�������ִ�е�   /// </summary>
        bool desiredJump;

        void Awake()
        {
            velocity = Vector3.zero;
            body2D = GetComponent<Rigidbody2D>();
            characterInfo = GetComponent<Info.CharacterInfo>();
            minGroundDot = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
            if (characterInfo == null) Debug.LogError("��ɫ��ϢΪ��");
        }


        private void FixedUpdate()
        {
            //�������ݣ���������һ������֡�����ݽ��и���֮���
            UpdateState();

            //ȷ���ڿ��л����ڵ���
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

        /// <summary>   /// ������ƶ�ֵ��������2D�����ֻ��ˮƽ�ƶ�   /// </summary>
        /// <param name="horizontal">ˮƽ�ƶ�ֵ</param>
        public void Move(float horizontal)
        {
            horizontal = Mathf.Clamp(horizontal, -1.0f, 1.0f);
            desiredVelocity = Vector2.right * horizontal * characterInfo.runSpeed;
        }

        void UpdateState()
        {
            velocity = body2D.velocity;
            //�����ڵ���ʱִ���������淽��
            if (onGround/*�ڵ���*/)
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
            //ֻ��������ͬ�����б�Ҫ����
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
            //ȷ����Ծ����
            if (onGround)       //�ڵ����ϣ�������ƽ�������һ�����Ǻ�б��б����
            {
                //�ڵ��ϣ�ֱ�Ӹ��ݽӴ�����
                jumpDirction = contactNormal;
            }
            //���������˳�
            else return;

            //���������Ĵ�С��ȷ���ƶ��ٶȣ���֤��Ծ�߶�Ϊ���ǵ�Ŀ��߶�
            float jumpSpeed = Mathf.Sqrt(2f * -Physics.gravity.y * jumpHeight);
            //ȷ�����ߵ�˺�Ĵ�С���жϵ�ǰ�ٶ�����Ծ�ķ���Ĺ�ϵ
            float aligneSpeed = Vector2.Dot(velocity, jumpDirction);
            if (aligneSpeed > 0)    //�����ڲ�ͬ����
            {
                //��С��Ծ�߶ȣ���ֹ������Ծ���ӵõ��ܸߵ���Ծ��ֵ
                jumpSpeed = Mathf.Max(jumpSpeed - aligneSpeed, 0);
            }
            //���������Ծ�ٶ�
            velocity += jumpDirction * jumpSpeed;
        }

        public void DesireJump()
        {
            desiredJump = true;
        }


        /// <summary>  /// �Ӵ��˳�ʱ����һ�νӴ��淨��  /// </summary>
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
                    //��֤����ж���Ӵ���ʱ�ܹ���ȷ�Ļ�ȡ����
                    contactNormal += normal;
                    connectObj = collision.gameObject;
                }
                //�������ƶ����ƣ����Ǳ��⳹�׵Ĵ�ֱ��
                else if (upDot > -0.01f)
                {
                    connectObj = collision.gameObject;
                }

            }
        }

        /// <summary>
        /// �����ƶ�����������֤�ƶ��ķ���������ƽ���
        /// </summary>
        void AdjustVelocity()
        {
            Vector2 xAixs = ProjectDirectionOnPlane(Vector2.right, contactNormal);
            Vector2 yAxis = ProjectDirectionOnPlane(Vector2.up, contactNormal);

            Vector2 relativeVelocity = velocity - connectionVelocity;
            //ȷ��ʵ���������ƽ���ϵ�X�ƶ�ֵ
            float currentX = Vector2.Dot(relativeVelocity, xAixs);
            //ȷ��ʵ���������ƽ���ϵ�Z�ƶ�ֵ
            float currentY = Vector2.Dot(relativeVelocity, yAxis);

            //ȷ�����ٶȴ�С
            float acceleration = onGround ? groundAcceleration : airAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;


            ////ȷ�����������õ����ƶ�ֵ
            float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
            float newY = Mathf.MoveTowards(currentY, desiredVelocity.y, maxSpeedChange);
            velocity += xAixs * (newX - currentX) + yAxis * (newY - currentY);

            //if (!onGround)      //�ڿ���ʱ�����ƶ��ٶȣ������������ϰ�
            //{
            //    velocity.x += Mathf.Clamp(velocity.x, -1.0f, 1.0f) * 0.1f;
            //}
        }

        /// <summary>  /// ������ݣ���һЩ���ݹ�Ϊ��ʼ��  /// </summary>
        void ClearState()
        {
            onGround = false;
            contactNormal = connectionVelocity = Vector2.zero;
            preConnectObj = connectObj;
            connectObj = null;
        }

        /// <summary>    /// ȷ���÷���ͶӰ����ƽ���ϵķ���ֵ��������й���׼��    /// </summary>
        Vector2 ProjectDirectionOnPlane(Vector2 direction, Vector2 normal)
        {
            return (direction - normal * Vector2.Dot(direction, normal)).normalized;
        }

        /// <summary>   /// ��תģ��     /// </summary>
        void Rotate()
        {
            //��ת�����Ŀ���
            if (velocity.x != 0)
            {
                Vector3 rotateTarget = transform.position;
                rotateTarget += (velocity.x > 0) ? Vector3.forward : -Vector3.forward;
                transform.LookAt(rotateTarget);
            }

        }

    }
}