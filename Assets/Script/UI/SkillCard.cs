using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillCard : MonoBehaviour
{
    public void AddListener(int skillIndex)
    {
        GetComponent<Button>().onClick.AddListener(() =>{
            Unit player = Funcs.GetCurrentUnitPlay.Invoke();
            StartCoroutine(player.skillList[skillIndex].ActionSkill());
            Actions.CloseListSkill?.Invoke();
        });
    }
}
