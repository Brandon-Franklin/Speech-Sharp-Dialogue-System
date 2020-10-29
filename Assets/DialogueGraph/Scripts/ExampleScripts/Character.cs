using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string character_name;

    [Header("Vitality and Skills")]
    public int Vitality = 0;
    [Space(3)]
    public int Althetics = 0;
    public int Medicine = 0, Melee = 0;

    [Header("Agility and Skills")]
    public int Agility = 0;
    [Space(3)]
    public int Reflex = 0;
    public int Speed = 0, Aim = 0;

    [Header("Acuity and Skills")]
    public int Acuity = 0;
    [Space(3)]
    public int Convince = 0;
    public int Learn = 0, Willpower = 0;
}
