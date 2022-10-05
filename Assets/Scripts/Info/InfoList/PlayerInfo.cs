using System.Reflection;
using UnityEngine;

namespace Info
{

    public class PlayerInfo : CharacterInfo
    {
        [SerializeField]
        /// <summary>  /// ���ǵ�Ĭ�ϼ��ܣ����Բ�����ֵ   /// </summary>
        private string[] defaultSkill;

        [SerializeField]
        DefferedRender.PostFXSetting fXSetting;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (defaultSkill == null)
                return;
            Assembly assembly = Assembly.GetExecutingAssembly();
            Skill.SkillManage skillManage = GetComponent<Skill.SkillManage>();
            if (skillManage == null) return;
            string prefit = "Skill.";
            for (int i=0; i<defaultSkill.Length; i++)
            {
                Skill.SkillBase skillBase = (Skill.SkillBase)
                    assembly.CreateInstance(prefit + defaultSkill[i]);
                skillManage.AddSkill(skillBase);
            }
        }

        public override void modifyHp(int dealtaHp)
        {
            base.modifyHp(dealtaHp);
            //fXSetting.SetColorFilter(Color.Lerp(Color.white, Color.red, (float)hp / maxHP));
        }



        protected override void DealWithDeath()
        {
            hp = maxHP;
        }

        
    }
}