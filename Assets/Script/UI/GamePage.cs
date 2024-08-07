using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePage : MonoBehaviour
{
    [SerializeField] private Button attackBTN, defenseBTN, healBTN, skillBTN,closeBTN;
    [SerializeField] private GameObject Card;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject listEnemyPanel;
    [SerializeField] private GameObject listSkillPanel;
    [SerializeField] private GameObject winPanel, losePanel;
    private void Start()
    {
        closeBTN.onClick.AddListener(() => Actions.CloseListUnit?.Invoke());
    }
    private void OnEnable()
    {
        Actions.AddListenerToGameButton += AddListener;
        Actions.OpenListUnit += OpenListUnit;
        Actions.CloseListUnit += CloseEnemyPanel;
        Actions.IsDisableAllButton += DisableAllBTN;
        Actions.CloseListSkill += () => listSkillPanel.SetActive(false);
        Actions.OpenListSkill += () => listSkillPanel.SetActive(true);
        Actions.OnResultBattle += ResultBattle;
    }

    private void CloseEnemyPanel()
    {
        listEnemyPanel.SetActive(false);
    }

    private void OnDisable()
    {
        Actions.AddListenerToGameButton -= AddListener;
        Actions.OpenListUnit -= OpenListUnit;
        Actions.CloseListUnit -= CloseEnemyPanel;
        Actions.IsDisableAllButton -= DisableAllBTN;
        Actions.CloseListSkill -= () => listSkillPanel.SetActive(false);
        Actions.OpenListSkill -= () => listSkillPanel.SetActive(true);
        Actions.OnResultBattle -= ResultBattle;

    }
    private void ResultBattle(bool isWin)
    {
        if (isWin)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
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
        if (listEnemyPanel.activeInHierarchy) return;
        ACTORTYPE actorType = listunitTarget[0].actorType;
        Debug.Log(actorType);
        listEnemyPanel.SetActive(true);
        if (contentParent.childCount > 0) // menghapus list button pada list button pemilihan enemy
        {
            foreach (Transform item in contentParent)
            {
                Destroy(item.gameObject);
            }
        }
        for (int i = 0; i < listunitTarget.Count; i++)
        {
            GameObject go = Instantiate(Card, contentParent);
            go.GetComponentInChildren<Text>().text = listunitTarget[i]._character.charaData.unitName;
            go.GetComponent<UnitCard>().AddListener(i,actorType);
            
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

    public void ExitGame()
    {
        SceneManager.LoadScene("Hub");
    }
}
