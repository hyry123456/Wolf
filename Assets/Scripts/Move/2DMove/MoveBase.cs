using UnityEngine;

namespace Motor
{
    /// <summary>
    /// 为了支持多角色移动，同时由于只有一个控制类，因此我们需要将移动方法抽象出来，
    /// 具体的运动由实现类决定
    /// </summary>
    public abstract class MoveBase : MonoBehaviour
    {
        /// <summary> /// 2D移动的抽象方法，输入一个水平的左右移动值 /// </summary>
        public abstract void Move(float horizontal);
        /// <summary> /// 跳跃方法，不一定要实现，因为不是每一个角色都可以跳跃 /// </summary>
        public abstract void DesireJump();

        public abstract bool OnGround();
    }
}