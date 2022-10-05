using UnityEngine;


namespace Skill
{
    /// <summary>  /// �������ܣ������������ƶ������� /// </summary>
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