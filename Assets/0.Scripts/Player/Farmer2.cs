using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer2 : Player
{
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        Speed = 3f;
        BulletFireDelayTime = 0.2f;

        HP = MaxHP = 100;
        shieldSpeed = 50;

        exp = 0;
        maxExp = 100;

        level = 0;
        Ui.Instance.Level = level + 1;
    }
}
