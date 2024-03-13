using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
    public void AddListener(int value, UNITACTIONTYPE actionType)
    {
        GetComponent<Button>().onClick.AddListener(() =>{
            Unit player = Funcs.GetCurrentUnitPlay.Invoke();
            Actions.OnSelectedSkill?.Invoke(player.skillList[value]);
            Actions.OpenListEnemy?.Invoke(actionType);
            Actions.CloseListSkill?.Invoke();
        });
    }
}
