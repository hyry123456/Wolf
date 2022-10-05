using UnityEngine;


namespace Skill
{
    /// <summary>  /// 钩锁技能，将钩锁技能移动到这里 /// </summary>
    public class HookRope : SkillBase
    {
        //Motor.RigibodyMotor motor;
        Motor.Rigibody2DMotor motor;
        public HookRope()
        {
            expendSP = 0;
            nowCoolTime = 0;
            coolTime = 0;
            skillName = "Hook Rope";
            skillType = SkillType.LongDisAttack;
        }


        public override void OnSkillRelease(SkillManage mana)
        {
            if (motor == null)
                motor = mana.GetComponent<Motor.Rigibody2DMotor>();
            motor.TransferToPosition(HookRopeManage.Instance.Target, 1);
        }
    }
}