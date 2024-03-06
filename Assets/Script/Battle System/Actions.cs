using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Actions
{
    public static Action<UnityAction,UnityAction,UnityAction> AddListenerToGameButton;
    public static Action<int> AttackToEnemy;
    public static Action<int> OpenListEnemy;
    public static Action CloseListEnemy;
    public static Action<bool> IsDisableAllButton;
}
