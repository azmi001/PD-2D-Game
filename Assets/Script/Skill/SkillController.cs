using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    public GameObject skillPrefab;
    public Transform skillListContent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisplaySkill(Unit playerUnit)
    {
        foreach (Skill skill in playerUnit.skillList)
        {
            GameObject skillItem = Instantiate(skillPrefab,skillListContent);

            //skillItem.transform.GetChild(0).GetComponent<Image>().sprite = skill.icon;
            skillItem.GetComponentInChildren<Text>().text = skill.skillName;
            Debug.Log(skill.skillName);
        }
    }
}
