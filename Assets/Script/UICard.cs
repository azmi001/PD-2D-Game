using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    public void AddListener(int value,UNITACTIONTYPE actionType)
    {
        GetComponentInChildren<Text>().text = $"Enemy {value}"; 
        GetComponent<Button>().onClick.AddListener(() => 
        {
            Actions.OnSelectedEnemy?.Invoke(Funcs.GetAllEnemyUnit.Invoke()[value]);
            Actions.OnUnitUseAction?.Invoke(actionType, Funcs.GetCurrentUnitPlay.Invoke());
            Actions.CloseListEnemy?.Invoke();
        });
    }
}
