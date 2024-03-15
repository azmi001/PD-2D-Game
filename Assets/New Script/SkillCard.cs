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
            Actions.OpenListEnemy?.Invoke(Funcs.GetAllPlayerUnit.Invoke(),actionType);
            Actions.CloseListSkill?.Invoke();
        });
    }
    public void AddListener(Skill _skill)
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            StartCoroutine(_skill.UseSkillTry());
        });
    }
}
