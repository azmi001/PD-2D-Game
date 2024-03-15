using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUp : Skill
{
    public HEALTARGET healTarget;

    public Unit target1;
    public override IEnumerator UseSkill(Unit target)
    {
        yield break;
    }

    public override IEnumerator UseSkillTry()
    {
        switch (healTarget)
        {
            case HEALTARGET.SINGLE:
                yield return new WaitUntil(() => target1 != null);
                target1.Heal(skillHeal);
                break;
            case HEALTARGET.EVERYONE:
                List<Unit> allTarget = new();
                switch (Funcs.GetCurrentUnitPlay.Invoke().actorType)
                {
                    case ACTORTYPE.PLAYER:
                        allTarget = Funcs.GetAllPlayerUnit.Invoke();
                        break;
                    case ACTORTYPE.ENEMY:
                        allTarget = Funcs.GetAllEnemyUnit.Invoke();
                        break;
                    default:
                        break;
                }
                break;
                foreach (var item in allTarget)
                {
                    item.Heal(skillHeal);
                }
            default:
                break;
        }
    }
}
public enum HEALTARGET
{
    SINGLE,
    EVERYONE
}
