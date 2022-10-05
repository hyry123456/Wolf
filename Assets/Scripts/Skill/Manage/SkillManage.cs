
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    public class SkillManage : MonoBehaviour
    {
        [SerializeField]
        /// <summary>        /// �����ͷ��ߵļ����б�        /// </summary>
        protected List<SkillBase> skills;
        /// <summary>        /// ���������ͷ��ߵ����м��ܣ����ǲ������޸�        /// </summary>
        public List<SkillBase> Skills
        {
            get
            {
                return skills;
            }
        }

        private Info.CharacterInfo characterInfo;
        /// <summary>        /// �����ͷ��ߵļ�����Ϣ����        /// </summary>
        public Info.CharacterInfo CharacterInfo
        {
            get
            {
                if(characterInfo == null)
                    characterInfo = GetComponentInChildren<Info.CharacterInfo>();
                return characterInfo;
            }
        }

        protected virtual void Start()
        {
            characterInfo = GetComponent<Info.CharacterInfo>();
        }

        protected void FixedUpdate()
        {
            if (skills == null) return;
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].nowCoolTime > 0)
                {
                    skills[i].nowCoolTime -= Time.fixedDeltaTime;
                }
            }
        }

        /// <summary>        /// ��鲢�ͷŸü���        /// </summary>
        /// <param name="skill">���ܶ���</param>
        public void CheckAndRelase(SkillBase skill)
        {
            if (skill == null) return;
            if(skill.nowCoolTime <= 0)
            {
                skill.nowCoolTime = skill.coolTime;
                skill.OnSkillRelease(this);
            }
        }

        /// <summary>       /// ��ü����б��е�ָ������        /// </summary>
        /// <param name="index">���ܱ��</param>
        /// <returns>���ܶ���</returns>
        public SkillBase GetSkillByIndex(int index)
        {
            if(index > 0 && skills != null && skills.Count > index)
            {
                SkillBase skill = skills[index];
                return skill;
            }
            return null;
        }

        /// <summary>        /// ������п���ʹ�õļ���        /// </summary>
        /// <returns>�����б�</returns>
        public List<SkillBase> GetCanUseSkill()
        {
            if (skills == null) return null;
            List<SkillBase> canUse = new List<SkillBase>(skills.Count);
            for(int i=0; i<skills.Count; i++)
            {
                if(skills[i].nowCoolTime <= 0)
                {
                    canUse.Add(skills[i]);
                }
            }
            return canUse;
        }

        /// <summary>
        /// ��ÿ���ʹ�õļ��ܣ��Ҽ��������봫�����Ͷ�Ӧ����Ҫע�����֧�ֶ�ƥ�䣬
        /// Ҳ����ͨ����λ�����ƥ����ּ�������
        /// </summary>
        /// <param name="type">��������</param>
        /// <returns>����ʹ�õļ����б�ע��Ϊ�յ����</returns>
        public List<SkillBase> GetCanUseSkillByType(SkillType type)
        {
            if (skills == null) return null;
            List<SkillBase> canUse = new List<SkillBase>();
            for(int i=0; i<skills.Count; i++)
            {
                if(skills[i].nowCoolTime <= 0 && (skills[i].skillType & type) != 0)
                {
                    canUse.Add(skills[i]);
                }
            }
            return canUse;
        }

        /// <summary> /// ��Ӽ��ܣ��������ƽ��м����޳��������ظ����   /// </summary>
        public void AddSkill(SkillBase skill)
        {
            if(skills == null)
            {
                skills = new List<SkillBase>();
                skills.Add(skill);
                return;
            }
            for(int i=0; i<skills.Count; i++)
            {
                if (skills[i].skillName == skill.skillName)
                    return;
            }
            skills.Add(skill);
        }
    }
}