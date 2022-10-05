using UnityEngine;


namespace Skill
{

    /// <summary>  
    /// 主角的近战攻击技能，挥舞一下球  
    /// </summary>
    public class WaveSickle : SkillBase
    {
        GameObject origin;  //根据的原对象
        Vector3 begin, end;
        float nowRadio = 0;
        Info.CharacterInfo character;
        Sphere_Pooling useObj;  //实际使用的对象
        Transform manaTran;
        Camera cam;

        /// <summary> /// 当前的时间 /// </summary>
        int state = 0;
        /// <summary>  /// 上一次释放的时间  /// </summary>
        float preTime = 0;
        /// <summary>  /// 技能的释放需要的时间   /// </summary>
        float relaseTime = 0.3f;
        /// <summary>   /// 连击允许的距离时间    /// </summary>
        float doubleHitDisTime = 1f;

        public WaveSickle()
        {
            expendSP = 0;
            nowCoolTime = 0;
            coolTime = 1f;
            skillName = "Wave Sickle";
            skillType = SkillType.NearDisAttack;
        }

        public override void OnSkillRelease(SkillManage mana)
        {
            if (origin == null)
                origin = Resources.Load<GameObject>("Prefab/Sphere_Pooling");
            if (character == null)
                character = mana.GetComponent<Info.CharacterInfo>();
            cam = Camera.main;
            if (cam == null) return;
            manaTran = mana.transform;


            if (state == 0 && Time.time - preTime < relaseTime + doubleHitDisTime)
            {
                state++;
            }
            else state = 0;
            preTime = Time.time;

            LoadBeginPosAndEndPos();

            useObj = (Sphere_Pooling)Common.SceneObjectPool.Instance.GetObject(
                "Sphere_Pooling", origin, begin, manaTran.position);        //朝向我们这边，方便碰撞检测
            useObj.collsionEnter = (Collision2D collision) =>
            {
                Info.CharacterInfo character = collision.gameObject.GetComponent<Info.CharacterInfo>();
                Debug.Log("Attack");
                if (character == null) return;
                character.modifyHp(-10);
            };

            nowRadio = 0;
            Common.SustainCoroutine.Instance.AddCoroutine(WaveSickleSustain, false);
        }

        bool WaveSickleSustain()
        {
            nowRadio += Time.deltaTime * (1.0f / relaseTime);
            if(!useObj.gameObject.activeSelf)
            {
                return true;
            }
            else if(nowRadio > 1 || cam == null)
            {
                useObj.CloseObject();
                return true;
            }

            //begin = manaTran.position + (manaTran.transform.right
            //    + Vector3.up) * character.nearAttackDistance;
            //end = manaTran.position + (manaTran.transform.right
            //    + -Vector3.up * 0.1f) * character.nearAttackDistance;

            LoadBeginPosAndEndPos();

            useObj.transform.position = Vector3.Lerp(begin, end, nowRadio);
            return false;
        }

        void LoadBeginPosAndEndPos()
        {
            switch (state)
            {
                case 0:
                    begin = manaTran.position + (manaTran.transform.right
                        + Vector3.up) * character.nearAttackDistance;
                    end = manaTran.position + (manaTran.transform.right
                        + -Vector3.up * 0.1f) * character.nearAttackDistance;
                    break;
                default:
                    begin = manaTran.position + (manaTran.transform.right
                        + -Vector3.up * 0.1f) * character.nearAttackDistance;
                    end = manaTran.position + (manaTran.transform.right
                        + Vector3.up) * character.nearAttackDistance;
                    break ;
            }

        }

    }
}