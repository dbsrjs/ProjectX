using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour   //abstract : 추상 클래스
{
    private Player p;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] expPrefab;
    
    protected float atkTime;    //공격 속도
    protected int power;
    protected int hp;

    private float atkTimer;

    private float hitFrezeTimer;    //일시 정지

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.play)
            return;

        if (p == null || hp <= 0)
            return;

        if (hitFrezeTimer > 0)     //0.5초 동안 정지
        {            
            hitFrezeTimer -= Time.deltaTime;
            return;
        }

        float x = p.transform.position.x - transform.position.x;

        sr.flipX = x < 0 ? true : x == 0 ? true : false;    //타겟(플레이어) 위치에 따라 보는 방향 변경

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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Shield>())   //Shield(삽)과 충돌
        {
            Dead(0.2f, 30);
        }

        else if(collision.GetComponent<Bullet>())    //총알과 충돌
        {
            collision.GetComponent<Bullet>().HitCount++;
            if (collision.GetComponent<Bullet>().HitCount >= collision.GetComponent<Bullet>().HitMaxCount)
            {
                Destroy(collision.gameObject);
            }
            Dead(0.5f, 20);
        }
    }

    void Dead(float frezeTime, int damage)
    {
        hitFrezeTimer = frezeTime;
        hp -= damage;
        animator.SetTrigger("hit");
        AudioManager.instance.Play("Hit1");
        if (hp <= 0)    //죽으면
        {
            Destroy(GetComponent<Rigidbody2D>());    //Rigidbody2D 삭제
            GetComponent<CapsuleCollider2D>().enabled = false;  //CapsuleCollider2D OFF
            animator.SetTrigger("dead");
            StartCoroutine("CDropExp");
        }
    }

    IEnumerator CDropExp()
    {
        Ui.instance.KillCount++;
        yield return new WaitForSeconds(0.5f);    //0.5초 뒤에
        //경험치 아이템 드랍률

        int rand = Random.Range(0, 101);    //가중치 랜덤 확률

        if(rand < 70)
            Instantiate(expPrefab[0], transform.position, Quaternion.identity);
        else if (rand < 90)
            Instantiate(expPrefab[1], transform.position, Quaternion.identity);
        else
            Instantiate(expPrefab[2], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);    //2초 뒤에
        Destroy(gameObject);    //죽은 몬스터 삭제
    }
}
