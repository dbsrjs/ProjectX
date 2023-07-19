using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Player p;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject expPrefab;

    protected float atkTime = 2f;    //공격 속도
    protected int power = 10;
    protected int hp = 50;

    private float atkTimer;

    private float hitFrezeTimer;    //일시 정지

    // tart is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if (collision.GetComponent<Shield>())
        {
            hitFrezeTimer = 0.5f;   //일시 정지
            hp -= 10;
            if (hp <= 0)    //죽으면
            {
                Destroy(GetComponent<Rigidbody2D>());    //Rigidbody2D 삭제
                GetComponent<CapsuleCollider2D>().enabled = false;  //CapsuleCollider2D OF
                animator.SetTrigger("dead");
                //Invoke("DropExp", 1f);
                StartCoroutine("CDropExp");
            }
        }
    }

    IEnumerator CDropExp()
    {       
        yield return new WaitForSeconds(1f);    //1초 뒤에
        Instantiate(expPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);    //2초 뒤에
        Destroy(gameObject);    //죽은 몬스터 삭제
    }
}
