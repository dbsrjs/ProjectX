using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Transform shildPrefab;
    [SerializeField] private Transform shieldParent;

    [SerializeField] private Transform firePos;
    [SerializeField] private Bullet bullet;

    private List<Transform> shields = new List<Transform>();

    float bulletTimer;

    int shieldCount, shieldSpeed, level;
    float x, y, exp, maxExp;

    public int HP { get; set; }
    public int MaxHP { get; set; }
    public float Speed { get; set; }
    public int BulletHitMaxCount { get; set; }

    public float BulletFireDelayTime { get; set; }
    public float Exp
    {
        get { return exp; }
        set
        {
            exp = value;
            UI.instance.SetExp(ref exp, ref maxExp, ref level);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Speed = 3f; 
        BulletFireDelayTime = 0.2f;

        HP = MaxHP = 100;
        shieldSpeed = 50;

        exp = 0;
        maxExp = 100;

        level = 0;
        UI.instance.Level = level + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.instance.gamestate != GameState.Play)
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
    }

    public void Hit(int damage)    //damage = power(Monster) = 10;
    {
        HP -= damage;   //데미지 감소
        UI.instance.SetHP(HP, MaxHP);
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
