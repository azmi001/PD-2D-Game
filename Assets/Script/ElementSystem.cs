using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class ElementSystem : MonoBehaviour
{
 
    public static bool IsStrongAgainst(ElementType attacker, ElementType defender)
    {
        return (attacker == ElementType.Fire && defender == ElementType.Leaf) ||
               (attacker == ElementType.Leaf && defender == ElementType.Water) ||
               (attacker == ElementType.Water && defender == ElementType.Fire);
    }

    public static bool IsWeakAgainst(ElementType attacker, ElementType defender)
    {
        return (attacker == ElementType.Leaf && defender == ElementType.Fire) ||
               (attacker == ElementType.Water && defender == ElementType.Leaf) ||
               (attacker == ElementType.Fire && defender == ElementType.Water);
    }
}
