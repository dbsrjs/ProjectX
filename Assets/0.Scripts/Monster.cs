using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Player p;
    [SerializeField] private SpriteRenderer sr;

    protected float atkTime = 2f;    //���� �ӵ�
    protected int power = 10;

    private float atkTimer;
    // tart is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p == null)
        {
            return;
        }
        float x = p.transform.position.x - transform.position.x;

        sr.flipX = x < 0 ? true : x == 0 ? true : false;    //Ÿ�� ��ġ�� ���� ���� ���� ����

        float distance = Vector2.Distance(p.transform.position, transform.position);

        if (distance <= 1)   //����
        {
            atkTimer += Time.deltaTime;
            if (atkTimer > atkTime)
            {
                atkTimer = 0;
                p.Hit(power);
            }
        }
        else    //�̵�
        {
            Vector2 v1 = (p.transform.position - transform.position).normalized * Time.deltaTime * 2f;
            transform.Translate(v1);
        }
    }

    public void SetPlayer(Player p)
    {
        this.p = p;
    }
}
