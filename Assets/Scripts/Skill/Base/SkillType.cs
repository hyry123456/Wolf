
namespace Skill
{
    [System.Serializable]
    public enum SkillType 
    {
        /// <summary>        /// Զ�̹�������        /// </summary>
        LongDisAttack = 1,
        /// <summary>        /// ��ս����        /// </summary>
        NearDisAttack = 2,
        /// <summary>        /// �������ܣ����˺�֮���        /// </summary>
        Auxiliary = 4,
        /// <summary>        /// ���Ƽ���        /// </summary>
        Heal = 8,
        /// <summary>        /// ���ܼ���        /// </summary>
        Dodge = 16,
    }
}