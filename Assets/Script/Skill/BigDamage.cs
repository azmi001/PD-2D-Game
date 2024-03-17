using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Big Attack",menuName = "Skill/Big Attack")]
public class BigDamage : Skill
{
    public override IEnumerator ActionSkill()
    {
        Actions.OpenListUnit?.Invoke(Funcs.GetAllEnemyUnit());
        Actions.OnTargetedUnit += UseSkillOnTarget;
        yield return null;
    }

    private void UseSkillOnTarget(Unit target)
    {
        target.TakeDemage(skillDmg,target._def,skillElement);
        Actions.OnUnitUsedAction?.Invoke(Funcs.GetCurrentUnitPlay());
        Actions.OnTargetedUnit -= UseSkillOnTarget;
    }
}
