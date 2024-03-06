using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamePage : MonoBehaviour
{
    [SerializeField] private Button attackBTN, defenseBTN, healBTN, skillBTN;
    [SerializeField] private GameObject enemyCard;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject listEnemyPanel;
    private void OnEnable()
    {
        Actions.AddListenerToGameButton += AddListener;
        Actions.OpenListEnemy += OpenListEnemy;
        Actions.CloseListEnemy += () => listEnemyPanel.SetActive(false);
        Actions.IsDisableAllButton += DisableAllBTN;
    }


    private void OnDisable()
    {
        Actions.AddListenerToGameButton -= AddListener;
        Actions.OpenListEnemy -= OpenListEnemy;
        Actions.CloseListEnemy -= () => listEnemyPanel.SetActive(false);
        Actions.IsDisableAllButton -= DisableAllBTN;

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
    private void OpenListEnemy(int enemyCount)
    {
        listEnemyPanel.SetActive(true);
        if (contentParent.childCount > 0)
        {
            foreach (Transform item in contentParent)
            {
                Destroy(item.gameObject);
            }
        }
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject go = Instantiate(enemyCard, contentParent);
            go.SendMessage("AddListener",i);
        }
    }

    private void AddListener(UnityAction action1, UnityAction action2, UnityAction action3)
    {
        attackBTN.onClick.RemoveAllListeners();
        defenseBTN.onClick.RemoveAllListeners();
        healBTN.onClick.RemoveAllListeners();
        skillBTN.onClick.RemoveAllListeners();

        attackBTN.onClick.AddListener(action1);
        defenseBTN.onClick.AddListener(action2);
        healBTN.onClick.AddListener(action3);
    }
}
