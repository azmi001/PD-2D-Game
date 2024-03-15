using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamePage : MonoBehaviour
{
    [SerializeField] private Button attackBTN, defenseBTN, healBTN, skillBTN,closeBTN;
    [SerializeField] private GameObject Card;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject listEnemyPanel;
    [SerializeField] private GameObject listSkillPanel;
    private void Start()
    {
        closeBTN.onClick.AddListener(() => Actions.CloseListUnit?.Invoke());
    }
    private void OnEnable()
    {
        Actions.AddListenerToGameButton += AddListener;
        Actions.OpenListUnit += OpenListUnit;
        Actions.CloseListUnit += () => listEnemyPanel.SetActive(false);
        Actions.IsDisableAllButton += DisableAllBTN;
        Actions.CloseListSkill += () => listSkillPanel.SetActive(false);
        Actions.OpenListSkill += () => listSkillPanel.SetActive(true);
    }


    private void OnDisable()
    {
        Actions.AddListenerToGameButton -= AddListener;
        Actions.OpenListUnit -= OpenListUnit;
        Actions.CloseListUnit -= () => listEnemyPanel.SetActive(false);
        Actions.IsDisableAllButton -= DisableAllBTN;
        Actions.CloseListSkill -= () => listSkillPanel.SetActive(false);
        Actions.OpenListSkill -= () => listSkillPanel.SetActive(true);

    }
    private void DisableAllBTN(bool isDisable)
    {
        if (isDisable)
        {
            attackBTN.interactable = false;
            defenseBTN.interactable = false;
            healBTN.interactable = false;
            skillBTN.interactable = false;
        }
        else
        {

            attackBTN.interactable = true;
            defenseBTN.interactable = true;
            healBTN.interactable = true;
            skillBTN.interactable = true;
        }
    }
    private void OpenListUnit(List<Unit>listunitTarget)
    {
        ACTORTYPE actorType = listunitTarget[0].actorType;
        Debug.Log(actorType);
        listEnemyPanel.SetActive(true);
        if (contentParent.childCount > 0)
        {
            foreach (Transform item in contentParent)
            {
                Destroy(item.gameObject);
            }
        }
        for (int i = 0; i < listunitTarget.Count; i++)
        {
            GameObject go = Instantiate(Card, contentParent);
            go.GetComponentInChildren<Text>().text = listunitTarget[i].character.unitName;
            go.GetComponent<UnitCard>().AddListener(i,actorType);
            Debug.Log(Funcs.GetAllPlayerUnit.Invoke()[i]);
        }
    }

    private void AddListener(UnityAction action1, UnityAction action2, UnityAction action3,UnityAction action4)
    {
        attackBTN.onClick.RemoveAllListeners();
        defenseBTN.onClick.RemoveAllListeners();
        healBTN.onClick.RemoveAllListeners();
        skillBTN.onClick.RemoveAllListeners();

        attackBTN.onClick.AddListener(action1);
        defenseBTN.onClick.AddListener(action2);
        healBTN.onClick.AddListener(action3);
        skillBTN.onClick.AddListener(action4);
    }
}
