
namespace Skill {
    public interface ISkill
    {
        /// <summary>        /// �����ͷŷ���        /// </summary>
        /// <param name="mana">����ӵ����</param>
        void OnSkillRelease(SkillManage mana);
    }
}