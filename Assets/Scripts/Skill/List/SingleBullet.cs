
using UnityEngine;

namespace Skill
{
    public class SingleBullet : SkillBase
    {
        GameObject originBullet;

        public SingleBullet()
        {
            expendSP = 0;
            nowCoolTime = 0;
            coolTime = 1;
            skillName = "Single Bullet";
            skillType = SkillType.LongDisAttack;
            originBullet = Resources.Load<GameObject>("Prefab/poolingBullet");
        }

        public override void OnSkillRelease(SkillManage mana)
        {
            Bullet_Pooling bullet_Pooling =
                (Bullet_Pooling)Common.SceneObjectPool.Instance.GetObject("Pooling_Bullet",
                originBullet, mana.transform.position + mana.transform.right * 0.2f,
                mana.transform.position + mana.transform.right * 0.6f);
            bullet_Pooling.attackTargetTag = "Enemy";
        }
    }
}