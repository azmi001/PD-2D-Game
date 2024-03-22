using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UnitCard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    int index;
    ACTORTYPE aCTORTYPE;
    public void AddListener(int targetIndex,ACTORTYPE actorType)
    {
        index = targetIndex;
        aCTORTYPE = actorType;
        GetComponent<Button>().onClick.AddListener(()=>
        {
            switch (actorType)
            {
                case ACTORTYPE.PLAYER:
                    Actions.OnTargetedUnit?.Invoke(Funcs.GetAllPlayerUnit()[targetIndex]);
                    break;
                case ACTORTYPE.ENEMY:
                    Actions.OnTargetedUnit?.Invoke(Funcs.GetAllEnemyUnit()[targetIndex]);
                    break;
                default:
                    break;
            }
            Actions.OnShowHoverTarget?.Invoke(false, index, aCTORTYPE);
            Actions.CloseListUnit?.Invoke();
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Actions.OnShowHoverTarget?.Invoke(true, index, aCTORTYPE);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Actions.OnShowHoverTarget?.Invoke(false, index, aCTORTYPE);
    }
}
