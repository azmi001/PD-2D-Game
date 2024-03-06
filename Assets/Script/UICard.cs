using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    public void AddListener(int value)
    {
        Debug.Log("tombol ini menyerang enemy yang ke " + value);
        GetComponentInChildren<Text>().text = $"Enemy {value}"; 
        GetComponent<Button>().onClick.AddListener(() => 
        { 
            Actions.AttackToEnemy?.Invoke(value);
            Actions.CloseListEnemy?.Invoke();
        });
    }
}
