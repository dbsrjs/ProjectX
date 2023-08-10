using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer0 : Player   //남캐
{
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        Speed = 3f; //속도
        BulletFireDelayTime = 0.2f; //총알 연사속도

        HP = MaxHP = 100;   //HP
        shieldSpeed = 50;   //삽 속도

        exp = 0;
        maxExp = 100;

        level = 0;
        Ui.Instance.Level = level + 1;
    }
}
