using UnityEngine;

public abstract class BattleBaseState 
{
    public abstract void EnterState(BattleStateManager battle);
    public abstract void UpdateState(BattleStateManager battle);
}
