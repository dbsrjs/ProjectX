using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{

    [SerializeField] private Transform shildPrefab;
    [SerializeField] private Transform shieldParent;

    [SerializeField] private Transform firePos;
    [SerializeField] private Bullet bullet;

    protected Animator animator;
    protected SpriteRenderer sr;

    [SerializeField] private Transform boom;

    private List<Transform> shields = new List<Transform>();

    private float x, y;
    private int shieldCount;    //삽 갯수

    protected int  shieldSpeed, level;  //삽 속도, 플레이어 레벨
    protected float exp, maxExp, bulletTimer;   //현재 경험치, 최대 경험지, ??

    public int HP { get; set; } //현재 HP
    public int MaxHP { get; set; }  //최대 HP
    public float Speed { get; set; }    //플레이어 속도
    public int BulletHitMaxCount { get; set; }  //총알 관통 횟수

    public float BulletFireDelayTime { get; set; }  //총알 연사 속도  ??
    public float Exp
    {
        get { return exp; }
        set
        {
            exp = value;
            Ui.instance.SetExp(ref exp, ref maxExp, ref level);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.Play)    //GameState가 Play가 아니라면
            return;

        x = Input.GetAxis("Horizontal");    //플레이어 이동(A, D)
        y = Input.GetAxis("Vertical");      //플레이어 이동(W, S)

        transform.Translate(new Vector3(x, y, 0f) * Time.deltaTime * Speed);

        if((x == 0 && y == 0) && HP != 0)
        {
            animator.SetTrigger("idle");    //서있는 이미지
        }
        else if(HP != 0)
        {
            animator.SetTrigger("run");     //달리는 이미지
        }

        if(x != 0)
        {
            sr.flipX = x < 0 ? true : false;    //보는 방향 변경(삼항식)
        }

        shieldParent.Rotate(Vector3.back * Time.deltaTime * shieldSpeed);   //shield 오른쪽으로 회전

        Monster[] monsters = FindObjectsOfType<Monster>();
        List<Monster> atkMonsterList = new List<Monster>();
        bulletTimer += Time.deltaTime;

        if (monsters.Length > 0 && bulletTimer > BulletFireDelayTime)
        {
            foreach (Monster m in monsters)
            {
                float distance = Vector3.Distance(transform.position, m.transform.position);
                if(distance < 4)
                {
                    atkMonsterList.Add(m);
                }
            }

            if(atkMonsterList.Count > 0)
            {
                Monster m = atkMonsterList[Random.Range(0, atkMonsterList.Count)];

                // 타켓을 찾아 방향 전환
                Vector2 vec = transform.position - m.transform.position;
                float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                firePos.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);

                Bullet b = Instantiate(bullet, firePos);    //bullet 생성
                b.SetHitMaxCount(BulletHitMaxCount + 1);
                b.transform.SetParent(null);
            }
            bulletTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            BulletHitMaxCount++;
        }

        if (Input.GetKeyDown(KeyCode.E))    //폭탄 설치
        {
            Instantiate(boom, transform.position, transform.rotation);
        }
    }

    public void Hit(int damage)    //damage = power(Monster) = 10;
    {
        if (Ui.instance.gamestate != GameState.Play)
            return;

        HP -= damage;   //HP 감소
        Ui.instance.SetHP(HP, MaxHP);

        if (HP <= 0)    //죽었을 때
        {
            Ui.instance.ShowDeadPopup(level + 1);
            animator.SetTrigger("dead");

            Invoke("Dead", 2f);
        }
    }

    void Dead()
    {
        Ui.instance.ShowDeadPopup(level + 1);
    }

    public void Shield()
    {
        float z = 360 / shieldCount;
        for (int i = 0; i < shieldCount; i++)
        {
            shields[i].rotation = Quaternion.Euler(0, 0, z * i);    //Quaternion.Euler : 오브젝트를 회전시키는데 사용
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Item>())
        {
            collision.GetComponent<Item>().target = transform;     //target = player(Item)
            collision.GetComponent<Item>().isPickup = true;     //isPickup을 true로 변경
        }

        if (collision.gameObject.name == "Mag")
        {
            Item[] items = FindObjectsOfType<Item>();
            foreach (var item in items)
            {
                item.target = transform;
                item.isPickup = true;
            }
        }
    }

    public void AddShild()    //방패 증가
    {
        shieldCount++;
        shields.Add(Instantiate(shildPrefab, shieldParent));    //shield 생성
        Shield();
    }
}
