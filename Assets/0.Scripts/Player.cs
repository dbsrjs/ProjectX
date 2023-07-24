using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] private float speed = 4f;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Transform ShieldPrefab;
    [SerializeField] private Transform shieldParent;

    [SerializeField] private Transform firePos;
    [SerializeField] private Bullet bullet;

    private List<Transform> shields = new List<Transform>();

    float bulletTimer;

    int hp, maxhp, shieldCount, shieldSpeed,level;
    float x, y, exp, maxpExp;

    public int BulletHitMaxCount { get; set; }

    public float Exp
    {
        get { return exp; }
        set
        {
            exp = value;
            Ui.instance.SetExp(ref exp, ref maxpExp, ref level);
        }
    }

    public float maxExp
    {
        set
        {
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        hp = maxhp = 100;
        shieldSpeed = 50;

        exp = 0;
        maxExp = 100;

        level = 0;
        Ui.instance.Level = level + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.play)
            return;

        x = Input.GetAxis("Horizontal");    //플레이어 이동(A. D)
        y = Input.GetAxis("Vertical");      //플레이어 이동(W, S)

        transform.Translate(new Vector3(x, y, 0f) * Time.deltaTime * speed);

        if ((x == 0 && y == 0) && hp != 0)
        {
            animator.SetTrigger("idle");    //서있는 이미지
        }
        else if(hp != 0)
        {
            animator.SetTrigger("run");    //달리는 이미지
        }

        if (x != 0)
        {
            sr.flipX = x < 0 ? true : false;    //보는 방향 변경
        }

        if(Input.GetKeyDown(KeyCode.F1))    //shield 개수 증가
        {
            shieldCount++;
            shields.Add(Instantiate(ShieldPrefab, shieldParent));
            Shield();
        }

        if (Input.GetKeyDown(KeyCode.F2))   //shield 속도 증가
        {
            shieldSpeed += 10;
        }

        shieldParent.Rotate(Vector3.back * Time.deltaTime * shieldSpeed);   //shield 오른쪽으로 회전

        Monster[] monsters = FindObjectsOfType<Monster>();
        List<Monster> atkMonsterList = new List<Monster>();
        bulletTimer += Time.deltaTime;

        if (monsters.Length > 0 && bulletTimer > 2f)
        {
            foreach (Monster m in monsters)
            {
                float distance = Vector3.Distance(transform.position, m.transform.position);
                if (distance < 4)
                {
                    atkMonsterList.Add(m);
                }
            }

            if (atkMonsterList.Count > 0)
            {
                Monster m = atkMonsterList[Random.Range(0, atkMonsterList.Count)];

                //타겟을 찾아 방향 전환
                Vector2 vec = transform.position - m.transform.position;
                float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                firePos.rotation = Quaternion.AngleAxis(angle - 180, Vector3.back);
                Bullet b = Instantiate(bullet, firePos);
                b.SetHitMaxCount(BulletHitMaxCount + 1);
                b.transform.SetParent(null);
            }
            bulletTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            //bullet.
        }
    }

    public void Hit(int damage)    //damage = power(Monster) = 10;
    {
        hp -= damage;
        Ui.instance.SetHp(hp, maxhp);
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
        if (collision.GetComponent<Item>())
        {
            collision.GetComponent<Item>().target = transform;  //target 지정(Player)
            collision.GetComponent<Item>().isPickup = true;
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
}
