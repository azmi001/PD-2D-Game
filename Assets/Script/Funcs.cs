using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Funcs
{
    public static Func<List<Unit>> GetAllPlayerUnit;
    public static Func<Unit> GetCurrentUnitPlay;
    public static Func<List<Unit>> GetAllEnemyUnit;
    public static Func<Akun> GetAkun;
    public static Func<DatabaseUnit> GetDatabaseUnit;
    public static Func<DatabaseSOCharacter> GetDatabaseSOCharacter;
    public static Func<StoryQuest> GetCurrentQuest;
}
