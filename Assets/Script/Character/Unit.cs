using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Unit : MonoBehaviour
{
    [Header("UI Stat Character")]
    public TMP_Text ChLvlText;
    public TMP_Text ChNmText;
    public Slider ChHpSlider;

    //data darah yang sekarang akan terupdate dalam game
    [Header("Status Darah")]
    public int currentHP;

    [Header("Status Level Charcter")]
    public float currentXP;
    public float expToLvUp;
    public float currentLv;

    //Merenfrensikan dari scriptable object Character untuk mengambil data stat character
    [Header("Data Character")]
    public Character character;
    [HideInInspector]public int _def;

    [Header("List Skill Character")]
    public List<Skill> skillList;

    //pengkodisian animasi def
    public bool isdefup = false;
    public int LastTurn = 0;
    public int CurrentTurn = 0;

    public ACTORTYPE actorType;

    public CharacterState characterState;

    //inisialisasi awal darah
    public void Awake()
    {
        InitializedData();
        SetHud();
    }

    public void Start()
    {
        _def = character.deffense;
        float hasil = MathF.Pow(2f / 0.09f, 1.6f);
        Debug.Log(hasil);
    }
    public void InitializedData()
    {
        currentHP = character.maxHP;
        currentLv = character.unitLevel;
        currentXP = character.unitexp;
        skillList = new List<Skill>(character.skills);
    }

    //logika penyerangan
    public bool TakeDemage(int dmg, int def, ElementType attackerElement)
    {
        //Mendubug dmg awal
        Debug.Log("Demage Murni " + character.unitName + "yang belum dicampur elemen " + dmg);

        //inisialisasi awal logika sitem dmg elemen
        int actualDamage = dmg * 15 /100;
        Debug.Log("Demage elemen didapat " + character.unitName + "adalah " + actualDamage);

        //fsm logic kalkulasi demg element
        switch (character.thisUnitElement)
        {
            case ElementType.Fire:
                if (attackerElement == ElementType.Leaf)
                    actualDamage *= -1; // Double damage
                else if (attackerElement == ElementType.Water)
                    actualDamage *= 1; // Half damage
                break;
            case ElementType.Leaf:
                if (attackerElement == ElementType.Water)
                    actualDamage *= -1; // Double damage
                else if (attackerElement == ElementType.Fire)
                    actualDamage *= 1; // Half damage
                break;
            case ElementType.Water:
                if (attackerElement == ElementType.Fire)
                    actualDamage *= -1; // Double damage
                else if (attackerElement == ElementType.Leaf)
                    actualDamage *= 1; // Half damage
                break;
        }

        //total dmg yang sudah ditambah dari dmg element
        int finalDmg = dmg + actualDamage;

        //mendebug total dmg final dmg
        Debug.Log("Demage Final " + character.unitName + "yang dicampur elemen " + finalDmg);

        //mebuat logika variasi dmg 20% +- dari total finaldmg
        int varian = finalDmg * 20/ 100;
        int minVarian = -varian;
        int maxVarian = varian;
        int result = UnityEngine.Random.Range(minVarian, maxVarian);

        //mendebug nial variasi dmg varian
        Debug.Log(character.unitName + "Min range -varian dari dmg " + minVarian);
        Debug.Log(character.unitName + "Min range +varian dari dmg " + maxVarian);
        Debug.Log(character.unitName +"Variasi dmg tambahan +- " + result);

        //mendebug nilai finaldmg yang ditambah oleh nilai variasi 
        int totalDmg = finalDmg + result;
        Debug.Log("Total Demage yang diberikan oleh " + character.unitName + "adalah " + totalDmg);

        //logika rumus pengurangan darah target 
        int finalDmg1 = ((finalDmg + result) * 4);
        int def1 = (def * 2);

        //menbuat logika jika dmg nya minus gak akan menambah darah target yang diserang
        //dan dmg yang diterima adalah 0
        if (def1 >= finalDmg1)
        {
            def1 = finalDmg1;
        }

        currentHP -= finalDmg1 - def1;
        //currentHP -= ((finalDmg + result) * 4) - (def * 2);

        

        //pengkondisian apakah target yang diserah sudah mati atau belum
        if (currentHP <= 0)
        {
            //mengupdate saat ui darah habis saat kondisi mati
            ChHpSlider.value = currentHP;

            //memutar animasi character mati
            StartCoroutine(unitdead());

            return true;
            
        }
        else
        {
            ChHpSlider.value = currentHP;
            //Memutar animasi character hurt
            if(isdefup)
            {
                GetComponentInChildren<Animator>().Play("Block");
            }
            else
            {
                GetComponentInChildren<Animator>().Play("Hurt");
            }

            return false;
        }
    }

    //memutar animasi character mati
    private IEnumerator unitdead()
    {
        GetComponentInChildren<Animator>().Play("KO");
        /*while (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }*/

        yield return new WaitForSeconds(1f);
        Actions.OnUnitDied?.Invoke(this);
    }

    public void DefUp(int amount)
    {
        _def *= amount;
        isdefup = true;
    }

    public void DefDefault()
    {
        _def = character.deffense;
        isdefup = false;
    }

    //logika skill heal
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > character.maxHP)
            currentHP = character.maxHP;
        ChHpSlider.value = currentHP;

    }
    

    public void GainExp(int exp)
    {
        currentXP += exp;
        if(currentXP >= expToLvUp)
        {
            LvUp();
        }
    }

    public void LvUp()
    {
        currentLv++;
        currentXP -= expToLvUp; 
        expToLvUp = CalculateNextLevelXP();
    }

    float CalculateNextLevelXP()
    {
        float result = Mathf.Pow(currentLv + 1 / 0.09f, 1.6f);
        Debug.Log("Result: " + result);
        return result;
    }

    public void SetHud()
    {
        ChNmText.text = character.unitName;
        ChLvlText.text = "" + currentLv;
        ChHpSlider.maxValue = character.maxHP;
        ChHpSlider.value = currentHP;
    }

    public void UnitAction()
    {
        switch (actorType)
        {
            case ACTORTYPE.PLAYER:
                break;
            case ACTORTYPE.ENEMY:
                StartCoroutine(ChangeStateInit());
                Debug.Log(characterState);
                break;
        }
    }

    // FSM NPC mengatur pergerakan penyerangan Enemy secara auto matis berdasrakan kelemahan element character player
    IEnumerator ChangeStateInit()
    {
        //inialisasi
        ChangeStateEnemy(CharacterState.IDLE);
        yield return new WaitForSeconds(1);
        
        //merandom state attack/heal/defense
        int rand = UnityEngine.Random.Range(1, System.Enum.GetValues(typeof(CharacterState)).Length);
        ChangeStateEnemy((CharacterState)rand);

    }

    public void ChangeStateEnemy(CharacterState state)
    {
        characterState = state;

        //mengabil data element player
        List<Unit> playerList = Funcs.GetAllPlayerUnit();

        //memilah data element player agar jika terdapat element yang sama pada
        // 1 team makan penyerangannya bisa dirandom bukan berdasarakan urutan array
        List<Unit> chElementRndm = new List<Unit>();

        //Logic FSM NPC Enemy
        switch (characterState)
        {
            case CharacterState.IDLE:
                GetComponentInChildren<Animator>().Play("Idle");// Memutar Animasi Idle

                break;
            case CharacterState.ATTACK:
                GetComponentInChildren<Animator>().Play("Attack");// Memutar Animasi Attack
                Unit TargetUnit = new Unit();

                //FSM logic Element Attact Target
                switch (character.thisUnitElement)
                {
                    case ElementType.Fire:
                        TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Leaf);//Mencari Element yang diuntungkan
                        Debug.Log("character State" + TargetUnit != null);
                        if (TargetUnit == null)
                        {
                            TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Fire);//mencari element yang sama
                            if (TargetUnit == null)
                            {
                                //logic random
                                foreach (var item in playerList)
                                {
                                    if (item.character.thisUnitElement == ElementType.Water)//element yang dirugikan
                                    {
                                        chElementRndm.Add(item);
                                    }
                                }
                                TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                            }
                            else
                            {
                                //logic random
                                foreach (var item in playerList)
                                {
                                    if (item.character.thisUnitElement == ElementType.Fire)//element yang sama
                                    {
                                        chElementRndm.Add(item);
                                    }
                                }
                                TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                            }
                        }
                        else
                        {
                            //logic random
                            foreach (var item in playerList)
                            {
                                if (item.character.thisUnitElement == ElementType.Leaf)//element yang diuntungkan
                                {
                                    chElementRndm.Add(item);
                                }
                            }
                            TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                        }
                        break;
                    case ElementType.Leaf:
                        TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Water);//Mencari Element yang diuntungkan
                        Debug.Log("character State" + TargetUnit != null);
                        /*if (TargetUnit == null)
                        {
                            TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Leaf);

                            if (TargetUnit == null)
                            {
                                TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Fire);
                            }
                        }*/
                        if (TargetUnit == null)
                        {
                            TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Leaf);//mencari element yang sama
                            if (TargetUnit == null)
                            {
                                //logic random
                                foreach (var item in playerList)
                                {
                                    if (item.character.thisUnitElement == ElementType.Fire)//element yang dirugikan
                                    {
                                        chElementRndm.Add(item);
                                    }
                                }
                                TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                            }
                            else
                            {
                                //logic random
                                foreach (var item in playerList)
                                {
                                    if (item.character.thisUnitElement == ElementType.Leaf)//element yang sama
                                    {
                                        chElementRndm.Add(item);
                                    }
                                }
                                TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                            }
                        }
                        else
                        {
                            //logic random
                            foreach (var item in playerList)
                            {
                                if (item.character.thisUnitElement == ElementType.Water)//element yang diuntungkan
                                {
                                    chElementRndm.Add(item);
                                }
                            }
                            TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                        }
                        break;
                    case ElementType.Water:
                        TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Fire);//Mencari Element yang diuntungkan
                        Debug.Log("character State" + TargetUnit != null);
                        /*if (TargetUnit == null)
                        {
                            TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Water);

                            if (TargetUnit == null)
                            {
                                TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Leaf);
                            }
                        }*/
                        if (TargetUnit == null)
                        {
                            TargetUnit = Array.Find(playerList.ToArray(), T => T.character.thisUnitElement == ElementType.Water);//mencari element yang sama
                            if (TargetUnit == null)
                            {
                                //logic random
                                foreach (var item in playerList)
                                {
                                    if (item.character.thisUnitElement == ElementType.Leaf)//element yang dirugikan
                                    {
                                        chElementRndm.Add(item);
                                    }
                                }
                                TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                            }
                            else
                            {
                                //logic random
                                foreach (var item in playerList)
                                {
                                    if (item.character.thisUnitElement == ElementType.Water)//element yang sama
                                    {
                                        chElementRndm.Add(item);
                                    }
                                }
                                TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                            }
                        }
                        else
                        {
                            //logic random
                            foreach (var item in playerList)
                            {
                                if (item.character.thisUnitElement == ElementType.Fire)//element yang diuntungkan
                                {
                                    chElementRndm.Add(item);
                                }
                            }
                            TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                        }
                        break;
                }
                TargetUnit.TakeDemage(character.damage, TargetUnit._def, character.thisUnitElement);
                break;
            case CharacterState.HEAL:
                Heal(100);//jumlah heal nya
                break;
            case CharacterState.DEFENSE:
                DefUp(3);//jumlah heal yang dikali dari basestate
                break;
        }
    }
}
    public enum ACTORTYPE
{
    PLAYER,
    ENEMY
}

