using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitActionProcess : MonoBehaviour
{
    private Unit unitTarget;
    private Skill selectedSkill;

    [SerializeField]private UNITACTIONTYPE unitActionType;
    private void OnEnable()
    {
        Actions.OnSelectedEnemy += (enemy) => unitTarget = enemy;
        Actions.OnSelectedSkill += (skill) => selectedSkill = skill;
        Actions.OnUnitUseAction += OnUnitUseAction;
    }


    private void OnDisable()
    {
        Actions.OnSelectedEnemy -= (enemy) => unitTarget = enemy;
        Actions.OnSelectedSkill -= (skill) => selectedSkill = skill;
        Actions.OnUnitUseAction -= OnUnitUseAction;
    }
    private void OnUnitUseAction(UNITACTIONTYPE _action, Unit unit)
    {
        switch (_action)
        {
            case UNITACTIONTYPE.ATTACK:
                bool isUnitDead = unitTarget.TakeDemage(unit.character.damage, unitTarget._def, unit.character.thisUnitElement);
                if (isUnitDead)
                {
                    Unit temp = unitTarget;
                    switch (unitTarget.actorType)
                    {
                        case ACTORTYPE.PLAYER:
                            Funcs.GetAllPlayerUnit.Invoke().Remove(unitTarget);
                            break;
                        case ACTORTYPE.ENEMY:
                            Funcs.GetAllEnemyUnit.Invoke().Remove(unitTarget);
                            break;
                        default:
                            break;
                    }
                    Destroy(temp.gameObject);
                }
                break;
            case UNITACTIONTYPE.HEAL:
                unit.Heal(unit.character.Heal);
                break;
            case UNITACTIONTYPE.DEFENSE:
                unit.DefUp(3);
                break;
            case UNITACTIONTYPE.SKILL:
                StartCoroutine(selectedSkill.UseSkill(unitTarget));
                break;
            default:
                break;
        }
        Actions.OnUnitUsedAction?.Invoke(unit);
        Actions.OnBattleStateChange?.Invoke(BattleState.CHECK);
    }
}

//bool isUnitDead = enemyUnit[value].TakeDemage(playerUnit[playerIndex].character.damage, enemyUnit[value]._def, playerUnit[playerIndex].character.thisUnitElement);
//if (isUnitDead)
//{
//    GameObject go = enemyUnit[value].gameObject;
//    enemyUnit.Remove(enemyUnit[value]);
//    Destroy(go);
//}