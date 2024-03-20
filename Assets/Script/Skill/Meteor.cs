using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Meteor", menuName = "Skill/Meteor")]
public class Meteor : Skill
{
    public override IEnumerator ActionSkill()
    {
        foreach (var item in Funcs.GetAllEnemyUnit())
        {
            item.TakeDemage(skillDmg, item._def, skillElement);
        }
        Actions.OnUnitUsedAction?.Invoke(Funcs.GetCurrentUnitPlay());
        yield return null;
    }
}
