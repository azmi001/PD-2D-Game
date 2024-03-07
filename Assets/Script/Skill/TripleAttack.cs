using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Triple Attack")]
public class TripleAttack : Skill
{
    public override IEnumerator UseSkill(Unit target)
    {
        bool isTargetDead = target.TakeDemage(skillDmg, target._def, skillElement);
        if (isTargetDead)
        {
            Unit temp = target;
            switch (target.actorType)
            {
                case ACTORTYPE.PLAYER:
                    Funcs.GetAllPlayerUnit.Invoke().Remove(target);
                    break;
                case ACTORTYPE.ENEMY:
                    Funcs.GetAllEnemyUnit.Invoke().Remove(target);
                    break;
                default:
                    break;
            }
            Destroy(temp.gameObject);
        }
        yield return null;
    }
}
