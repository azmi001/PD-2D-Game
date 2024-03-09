using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Big Attack",menuName = "Skill/Big Attack")]
public class BigDamage : Skill
{
    public override IEnumerator UseSkill(Unit target)
    {
        target.TakeDemage(skillDmg, target._def, skillElement);
        yield return null;
    }

    public override IEnumerator UseSkillTry()
    {
        throw new System.NotImplementedException();
    }
}
