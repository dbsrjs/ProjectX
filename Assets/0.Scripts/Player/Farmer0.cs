using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer0 : Player   //��ĳ
{
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        Speed = 3f; //�ӵ�
        BulletFireDelayTime = 0.2f; //�Ѿ� ����ӵ�

        HP = MaxHP = 100;   //HP
        shieldSpeed = 50;   //�� �ӵ�

        exp = 0;
        maxExp = 100;

        level = 0;
        Ui.Instance.Level = level + 1;
    }
}
