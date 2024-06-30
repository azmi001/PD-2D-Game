using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Actions
{
    //TOMBOL ATTACK, DEFENSE, HEAL, SKILL DIBERIKAN FUNGSI OLEH GAMESETTING MELALUI ACTION INI. 4 UNITY ACTION MEWAKILI 4 TOMBOL YANG ADA PADA GAME
    public static Action<UnityAction,UnityAction,UnityAction,UnityAction> AddListenerToGameButton;
    //MEMBUKA LIST UNIT YANG ADA DI GAME. BISA DIGUNAKAN OLEH PLAYER ATAUPUN ENEMY. LIST UNIT BISA BERISI LIST UNIT ENEMY ATAUPUN PLAYER
    public static Action<List<Unit>> OpenListUnit;
    //MENUTUP LIST UNIT SELEPAS DIGUNAKAN ATAU KETIKA TIDAK JADI DIGUNAKAN
    public static Action CloseListUnit;
    //MEMBUKA LIST SKILL YANG ADA PADA CURRENT CHARACTER
    public static Action OpenListSkill;
    //MENUTUP LIST SKILL
    public static Action CloseListSkill;
    //MENDISABLE SEMUA BUTTON.DIGUNAKAN KETIKA GILIRAN ENEMY JALAN. BOOLEAN UNTUK MENENTUKAN APAKAH IS DISABLE = TRUE/FALSE
    public static Action<bool> IsDisableAllButton;
    //MENENTUKAN BATTLE STATE SEKARANG ITU APA. PARAMETER BattleState MERUPAKAN ENUM YANG BERISI STATE YANG ADA PADA GAME. EX: WON/LOST/PLAYERTURN/ENEMYTURN/PAUSE.
    public static Action<BattleState> OnBattleStateChange;
    //BERFUNGSI KETIKA UNIT YANG SEDANG BERMAIN TELAH MELAKUKAN GILIRANNYA. PARAMETER UNIT MEWAKILI UNIT YANG SELESAI MELAKUKAN GILIRAN
    public static Action<Unit> OnUnitUsedAction;
    //BERFUNGSI KETIKA ADA UNIT YANG MATI. PARAMETER UNIT MEWAKILI UNIT YANG MATI.
    public static Action<Unit> OnUnitDied;
    //BERFUNGSI KETIKA TARGET SERANGAN ATAU SKILL SUDAH DITENTUKAN. PARAMETER UNIT MEWAKILI UNIT YANG AKAN MENJADI TARGET
    public static Action<Unit> OnTargetedUnit;
    //BERFUNGSI MEMUNCULKAN HOVER PADA UNIT. SEMENTARA UNIT AKAN BEWARNA HIJAU KETIKA MOUSE MEMILIH ENEMY 1,2,3. PARAMETER BOOL BERFUNGSI APAKAH MOUSE BERADA DI BUTTON TERSEBUT ATAU TIDAK, INT MENENTUKAN UNIT YANG KEBERAPA
    //YANG DILALUI OLEH MOUSE, ACTORTYPE MENENTUKAN APAKAH UNIT YANG DILALUI MOUSE BERUPA PLAYER ATAU ENEMY
    public static Action<bool,int,ACTORTYPE> OnShowHoverTarget;
    //MENGURANGI STAMINA UNTUK MELAKUKAN QUEST
    public static Action<StoryQuest> onQuestStart;
    //MEGNGECEK ON QUEST DONE
    public static Action<StoryQuest> onQuestFinis;
    public static Action ClaimStamina;
    public static Action<bool> OnResultBattle;

}
