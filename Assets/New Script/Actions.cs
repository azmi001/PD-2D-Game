using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Actions
{
    public static Action<UnityAction,UnityAction,UnityAction> AddListenerToGameButton;
    public static Action<UNITACTIONTYPE> OpenListEnemy;
    public static Action CloseListEnemy;
    public static Action CloseListSkill;
    public static Action<bool> IsDisableAllButton;
    public static Action<Skill> OnSelectedSkill;
    public static Action<Unit> OnSelectedEnemy;
    public static Action<UNITACTIONTYPE,Unit> OnUnitUseAction;
    public static Action<BattleState> OnBattleStateChange;
    public static Action<Unit> OnUnitUsedAction;
}
