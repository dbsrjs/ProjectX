using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Monster
{
    void Start()
    {
        atkTime = 1.2f;
        power = 30;
        hp = 120;
    }
}
