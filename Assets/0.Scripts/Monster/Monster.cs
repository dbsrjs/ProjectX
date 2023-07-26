using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour   //abstract : 추상 클래스
{
    private Player p;
    private Boom boom;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] expPrefab;

    protected float atkTime;    //공격 속도
    protected int power;    //공격력
    protected int hp;   //채력

    private float atkTimer;

    private float hitFrezeTimer;    //일시 정지

    // Update is called once per frame

    private void Start()
    {
        boom.GetComponent<Boom>().target = transform;
    }

    void Update()
    {
        if (UI.instance.gamestate != GameState.Play)    //GameState가 Play가 아니라면
            return;

        if (p == null || hp <= 0)   //플레이어가 없거나 죽었을 때
            return;

        if(hitFrezeTimer > 0)   //일시 정지 시간이 0 이상이라면
        {
            hitFrezeTimer -= Time.deltaTime;
            animator.SetTrigger("hit");    //hit 애니메이션 실행
            return;
        }
        else
        {
            animator.SetTrigger("run");    //run 애니메이션 실행
        }

        float x = p.transform.position.x - transform.position.x;

        sr.flipX = x < 0 ? true : x == 0 ? true : false;    //타겟(플레이어) 위치에 따라 보는 방향 변경, 삼항식

        float distance = Vector2.Distance(p.transform.position, transform.position);    //둘 사이의 거리 계산
        
        if (distance <= 1)  //거리가 1 이하라면 
        {
            atkTimer += Time.deltaTime;
            //공격
            if(atkTimer > atkTime)
            {
                atkTimer = 0;
                p.Hit(power);
            }
        }
        else
        {
            //이동
            Vector2 v1 = (p.transform.position - transform.position).normalized * Time.deltaTime * 1f;
            transform.Translate(v1);
        }
    }

    public void SetPlayer(Player p)
    {
        this.p = p;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Shild>())   //Shield(삽)과 충돌
        {
            Dead(0.2f, 30);
        }
        else if(collision.GetComponent<Bullet>())    //총알과 충돌
        {
            collision.GetComponent<Bullet>().HitCount++;    //HitCount 증가
            if (collision.GetComponent<Bullet>().HitCount >= collision.GetComponent<Bullet>().HitMaxCount)  //HitCount가 HitMaxCount보다 크다면
            {
                Destroy(collision.gameObject);     //Bullet 삭제
            }
            Dead(0.5f, 20);
        }
    }

    public void Dead(float frezeTime, int damage)
    {
        hitFrezeTimer = frezeTime;  //hitFrezeTimer 값 지정
        hp -= damage;   //데미지에 비례해서 hp 감소
        animator.SetTrigger("hit");    //hit 애니메이션 실행
        AudioManager.instance.Play("hit1");    //hit 소리

        if (hp <= 0)    //죽으면
        {
            Destroy(GetComponent<Rigidbody2D>());   //Rigidbody2D 삭제
            GetComponent<CapsuleCollider2D>().enabled = false;  //CapsuleCollider2D OFF
            animator.SetTrigger("dead");    //dead 애니메이션 실행
            StartCoroutine("CDropExp");    //CDropExp 코루틴 실행
        }
    }

    IEnumerator CDropExp()
    {
        UI.instance.KillCount++;    //KillCount Text 증가
        yield return new WaitForSeconds(1f);    //1초 후
        // 경험치 아이템 드랍률
        // TODO : 가중치 랜덤 확률로 변경 해야함...
        int rand = Random.Range(0, 101);    //0~100

        if(rand < 70)
            Instantiate(expPrefab[0], transform.position, Quaternion.identity);
        else if (rand < 90)
            Instantiate(expPrefab[1], transform.position, Quaternion.identity);
        else
            Instantiate(expPrefab[2], transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);    //2초 후
        Destroy(gameObject);    //몬스터 삭제
    }
}
