using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HealUp",menuName ="Skill/Heal")]
public class HealUp : Skill
{
    public HEALTARGET healTarget;
    public override IEnumerator ActionSkill()
    {
        switch (healTarget)
        {
            case HEALTARGET.SINGLE:
                Actions.OpenListUnit?.Invoke(Funcs.GetAllPlayerUnit());
                Actions.OnTargetedUnit += UseSkillOnTarget;
                break;
            case HEALTARGET.EVERYONE:
                foreach (var item in Funcs.GetAllPlayerUnit.Invoke())
                {
                    item.Heal(skillHeal);
                }
                break;
        }
        yield return null;
    }

    private void UseSkillOnTarget(Unit target)
    {
        Debug.Log(target == null);
        target.Heal(skillHeal);
        Actions.OnUnitUsedAction?.Invoke(Funcs.GetCurrentUnitPlay());
        Actions.OnTargetedUnit -= UseSkillOnTarget;
    }
}
public enum HEALTARGET
{
    SINGLE,
    EVERYONE
}
