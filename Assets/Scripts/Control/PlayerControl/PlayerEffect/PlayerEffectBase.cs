using UnityEngine;

namespace Control
{
    /// <summary>
    /// 角色效果基类，用来执行所有的角色效果行为
    /// </summary>
    public abstract class PlayerEffectBase : MonoBehaviour
    {
        public abstract void Begin();
        public abstract void End();
    }
}