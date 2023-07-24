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

        x = Input.GetAxis("Horizontal");    //�÷��̾� �̵�(A. D)
        y = Input.GetAxis("Vertical");      //�÷��̾� �̵�(W, S)

        transform.Translate(new Vector3(x, y, 0f) * Time.deltaTime * speed);

        if ((x == 0 && y == 0) && hp != 0)
        {
            animator.SetTrigger("idle");    //���ִ� �̹���
        }
        else if(hp != 0)
        {
            animator.SetTrigger("run");    //�޸��� �̹���
        }

        if (x != 0)
        {
            sr.flipX = x < 0 ? true : false;    //���� ���� ����
        }

        if(Input.GetKeyDown(KeyCode.F1))    //shield ���� ����
        {
            shieldCount++;
            shields.Add(Instantiate(ShieldPrefab, shieldParent));
            Shield();
        }

        if (Input.GetKeyDown(KeyCode.F2))   //shield �ӵ� ����
        {
            shieldSpeed += 10;
        }

        shieldParent.Rotate(Vector3.back * Time.deltaTime * shieldSpeed);   //shield ���������� ȸ��

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

                //Ÿ���� ã�� ���� ��ȯ
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
            shields[i].rotation = Quaternion.Euler(0, 0, z * i);    //Quaternion.Euler : ������Ʈ�� ȸ����Ű�µ� ���
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Item>())
        {
            collision.GetComponent<Item>().target = transform;  //target ����(Player)
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
