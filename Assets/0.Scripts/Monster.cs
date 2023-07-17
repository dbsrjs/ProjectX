using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Player p;
    [SerializeField] private SpriteRenderer sr;

    protected float atkTime = 2f;    //공격 속도
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

        sr.flipX = x < 0 ? true : x == 0 ? true : false;    //타겟 위치에 따라 보는 방향 변경

        float distance = Vector2.Distance(p.transform.position, transform.position);

        if (distance <= 1)   //공격
        {
            atkTimer += Time.deltaTime;
            if (atkTimer > atkTime)
            {
                atkTimer = 0;
                p.Hit(power);
            }
        }
        else    //이동
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
