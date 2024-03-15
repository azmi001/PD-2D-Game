using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Triple Attack")]
public class TripleAttack : Skill
{
    public override IEnumerator ActionSkill()
    {
        Actions.OpenListUnit.Invoke(Funcs.GetAllEnemyUnit());
        Actions.OnTargetedUnit += UseSkillOnTarget;
        yield return null;
    }

    private void UseSkillOnTarget(Unit obj)
    {
        obj.TakeDemage(skillDmg, obj._def, skillElement);
        Actions.OnUnitUsedAction?.Invoke(Funcs.GetCurrentUnitPlay());
        Actions.OnTargetedUnit -= UseSkillOnTarget;
    }

    public override IEnumerator UseSkill(Unit target)
    {
        target.TakeDemage(skillDmg, target._def, skillElement);
        yield return null;
    }
}
