using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class Unit : MonoBehaviour
{
    [Header("UI Stat Character")]
    public TMP_Text ChLvlText;
    public TMP_Text ChNmText;
    public Slider ChHpSlider;

    //data darah yang sekarang akan terupdate dalam game
    [Header("Status Darah")]
    public float currentHP;

    [Header("Status Level Charcter")]
    public float currentXP;
    public float expToLvUp;
    public float currentLv;

    //Merenfrensikan dari scriptable object Character untuk mengambil data stat character
    [Header("Data Character")]
    public Character character;
    public Character _character;
    public float _def;

    [Header("List Skill Character")]
    public List<Skill> skillList;

    //pengkodisian animasi def
    public bool isdefup = false;
    public int LastTurn = 0;
    public int CurrentTurn = 0;

    public ACTORTYPE actorType;

    public CharacterState characterState;

    public Transform opponentPos;

    //inisialisasi awal darah

    public void Start()
    {
        _character = ScriptableObject.Instantiate(character);
        Debug.Log(_character.charaData.damage);
        switch (actorType)
        {
            case ACTORTYPE.PLAYER:
                _character.charaData = Array.Find(Funcs.GetAkun().OwnedHeroes.ToArray(), t => t.unitName == _character.charaData.unitName);
                break;
            case ACTORTYPE.ENEMY:
                opponentPos.localPosition = new Vector3(-1.15f, 0, 0);
                break;
            default:
                break;
        }
        InitializedData();
        SetHud();
    }
    public void InitializedData()
    {
        _def = _character.charaData.deffense;
        currentHP = _character.charaData.maxHP;
        currentLv = _character.charaData.unitLevel;
        currentXP = _character.charaData.unitexp;
        skillList = new List<Skill>(_character.skills);
    }

    //logika penyerangan
    public bool TakeDemage(float dmg, float def, ElementType attackerElement)
    {
        //memutar sfx attack untuk player dan enemy
        AudioManager.instance.Play("Attack");

        //Mendubug dmg awal
        Debug.Log("Demage Murni " + _character.charaData.unitName + "yang belum dicampur elemen " + dmg);

        //inisialisasi awal logika sitem dmg elemen
        float actualDamage = dmg * 15 /100;
        Debug.Log("Demage elemen didapat " + _character.charaData.unitName + "adalah " + actualDamage);

        //fsm logic kalkulasi demg element
        switch (_character.thisUnitElement)
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
        float finalDmg = dmg + actualDamage;

        //mendebug total dmg final dmg
        Debug.Log("Demage Final " + _character.charaData.unitName + "yang dicampur elemen " + finalDmg);

        //mebuat logika variasi dmg 20% +- dari total finaldmg
        float varian = finalDmg * 20/ 100;
        float minVarian = -varian;
        float maxVarian = 0;
        float result = UnityEngine.Random.Range(minVarian, maxVarian);
        //mendebug nial variasi dmg varian
        Debug.Log(_character.charaData.unitName + "Min range -varian dari dmg " + minVarian);
        Debug.Log(_character.charaData.unitName + "Min range +varian dari dmg " + maxVarian);
        Debug.Log(_character.charaData.unitName +"Variasi dmg tambahan +- " + result);


        //mendebug nilai finaldmg yang ditambah oleh nilai variasi 
        float totalDmg = finalDmg + result;
        Debug.Log("Total Demage yang diberikan oleh " + _character.charaData.unitName + "adalah " + totalDmg);

        //logika rumus pengurangan darah target 
        float finalDmg1 = (finalDmg + result) * 2;

        float def1 = (def * 2);

        //mebuat logika variasi def 20% +- dari total finaldmg
        float varianDef = def1 * 20 / 100;
        float minVarianDef = -varianDef;
        float maxVarianDef = 0;
        float resultDef = UnityEngine.Random.Range(minVarianDef, maxVarianDef);
        //mendebug nial variasi def varian
        Debug.Log(_character.charaData.unitName + "Min range -varian dari Def " + minVarianDef);
        Debug.Log(_character.charaData.unitName + "Min range +varian dari Def " + maxVarianDef);
        Debug.Log(_character.charaData.unitName + "Variasi Def tambahan +- " + resultDef);

        Debug.Log(_character.charaData.unitName + "Sebelum pake variasi deff" + def1);
        def1 = def1 + resultDef;
        Debug.Log(_character.charaData.unitName + "Hasil dari def 1 + Variasi" + def1);
        Debug.LogWarning(finalDmg1);
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

            //memutar animasi _character.charaData mati
            StartCoroutine(unitdead());

            return true;
            
        }
        else
        {
            ChHpSlider.value = currentHP;
            //Memutar animasi _character.charaData hurt
            if(isdefup)
            {
                GetComponentInChildren<Animator>().Play("Block");
                //memutar sfx defense
                AudioManager.instance.Play("Defense");
            }
            else
            {
                GetComponentInChildren<Animator>().Play("Hurt");
            }

            return false;
        }
    }

    //memutar animasi _character.charaData mati
    private IEnumerator unitdead()
    {
        Debug.Log("Budi died");
        Actions.OnUnitDied?.Invoke(this);

        yield return null;
    }

    public void DefUp(int amount)
    {
        if (!isdefup)
        {
            _def *= amount;
            isdefup = true;
        }
    }

    public void DefDefault()
    {
        _def = _character.charaData.deffense;
        isdefup = false;
    }

    //logika skill heal
    public void Heal(float amount)
    {
        //memutar sfx heal
        AudioManager.instance.Play("Heal");

        //mebuat logika variasi Heal 20% +- dari total finaldmg
        float varianHeal = amount * 20 / 100;
        float minVarianHeal = -varianHeal;
        float maxVarianHeal = 0;
        float resultHeal = UnityEngine.Random.Range(minVarianHeal, maxVarianHeal);
        //mendebug nial variasi def varian
        Debug.Log(_character.charaData.unitName + "Min range -varian dari Heal " + minVarianHeal);
        Debug.Log(_character.charaData.unitName + "Min range +varian dari Heal " + maxVarianHeal);
        Debug.Log(_character.charaData.unitName + "Variasi Def tambahan +- " + resultHeal);

        Debug.Log(_character.charaData.unitName + "Sebelum pake variasi Heal" + amount);
        amount = amount + resultHeal;
        Debug.Log(_character.charaData.unitName + "Hasil dari Heal + Variasi" + amount);

        currentHP += amount;
        if (currentHP > _character.charaData.maxHP)
            currentHP = _character.charaData.maxHP;
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
        ChNmText.text = _character.charaData.unitName;
        ChLvlText.text = "" + currentLv;
        ChHpSlider.maxValue = _character.charaData.maxHP;
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

    public async void ChangeStateEnemy(CharacterState state)
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
                switch (_character.thisUnitElement)
                {
                    case ElementType.Fire:
                        TargetUnit = Array.Find(playerList.ToArray(), T => T._character.thisUnitElement == ElementType.Leaf);//Mencari Element yang diuntungkan
                        Debug.Log("character State" + TargetUnit != null);
                        if (TargetUnit == null)
                        {
                            TargetUnit = Array.Find(playerList.ToArray(), T => T._character.thisUnitElement == ElementType.Fire);//mencari element yang sama
                            if (TargetUnit == null)
                            {
                                //logic random
                                foreach (var item in playerList)
                                {
                                    if (item._character.thisUnitElement == ElementType.Water)//element yang dirugikan
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
                                    if (item._character.thisUnitElement == ElementType.Fire)//element yang sama
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
                                if (item._character.thisUnitElement == ElementType.Leaf)//element yang diuntungkan
                                {
                                    chElementRndm.Add(item);
                                }
                            }
                            TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                        }
                        break;
                    case ElementType.Leaf:
                        TargetUnit = Array.Find(playerList.ToArray(), T => T._character.thisUnitElement == ElementType.Water);//Mencari Element yang diuntungkan
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
                            TargetUnit = Array.Find(playerList.ToArray(), T => T._character.thisUnitElement == ElementType.Leaf);//mencari element yang sama
                            if (TargetUnit == null)
                            {
                                //logic random
                                foreach (var item in playerList)
                                {
                                    if (item._character.thisUnitElement == ElementType.Fire)//element yang dirugikan
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
                                    if (item._character.thisUnitElement == ElementType.Leaf)//element yang sama
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
                                if (item._character.thisUnitElement == ElementType.Water)//element yang diuntungkan
                                {
                                    chElementRndm.Add(item);
                                }
                            }
                            TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                        }
                        break;
                    case ElementType.Water:
                        TargetUnit = Array.Find(playerList.ToArray(), T => T._character.thisUnitElement == ElementType.Fire);//Mencari Element yang diuntungkan
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
                            TargetUnit = Array.Find(playerList.ToArray(), T => T._character.thisUnitElement == ElementType.Water);//mencari element yang sama
                            if (TargetUnit == null)
                            {
                                //logic random
                                foreach (var item in playerList)
                                {
                                    if (item._character.thisUnitElement == ElementType.Leaf)//element yang dirugikan
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
                                    if (item._character.thisUnitElement == ElementType.Water)//element yang sama
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
                                if (item._character.thisUnitElement == ElementType.Fire)//element yang diuntungkan
                                {
                                    chElementRndm.Add(item);
                                }
                            }
                            TargetUnit = chElementRndm[UnityEngine.Random.Range(0, chElementRndm.Count)];
                        }
                        break;
                }
                TargetUnit.TakeDemage(_character.charaData.damage, TargetUnit._def, _character.thisUnitElement);
                transform.position = TargetUnit.transform.position;
                VFXManager.instance.EffetPuch(TargetUnit.transform.position);// vfx keserang
                await WaitForAnimation(GetComponentInChildren<Animator>());
                transform.localPosition = Vector3.zero;
                break;
            case CharacterState.HEAL:
                Heal(_character.charaData.Heal);//jumlah heal 
                break;
            case CharacterState.DEFENSE:
                DefUp(3);//jumlah heal yang dikali dari basestate
                VFXManager.instance.EffetDef(transform.position);//vfx def
                break;
        }
    }
    private async Task WaitForAnimation(Animator anim)
    {
        // Mendapatkan panjang animasi dari clip yang sedang dimainkan
        float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;

        // Tunggu sesuai panjang animasi
        await Task.Delay((int)(animationLength * 1000));
    }

}
public enum ACTORTYPE
{
    PLAYER,
    ENEMY
}

