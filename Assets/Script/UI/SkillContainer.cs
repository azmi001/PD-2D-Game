using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillContainer : MonoBehaviour
{
    [SerializeField] private GameObject skillCard;
    [SerializeField] private Transform contentParent;

    [SerializeField] private List<Button> listSkillBTN = new();

    [SerializeField] private Button closeBTN;
    private void Start()
    {
        closeBTN.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        //menghapus semua skill yang ada dilist ketika skill container dibuka
        if (contentParent.childCount > 0)
        {
            foreach (Transform item in contentParent)
            {
                Destroy(item.gameObject);
            }
        }

        Unit player = Funcs.GetCurrentUnitPlay.Invoke();
        if (player.skillList != null || player.skillList.Count > 0)
        {
            if (player.skillList.Count <= 0 || player.skillList == null) return;
            for (int i = 0; i < player.skillList.Count; i++)
            {
                GameObject go = Instantiate(skillCard, contentParent);
                go.GetComponentInChildren<Text>().text = player.skillList[i].skillName;
                go.GetComponent<SkillCard>().AddListener(i);
            }
        }
    }
}
