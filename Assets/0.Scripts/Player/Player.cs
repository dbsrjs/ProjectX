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
    private int shieldCount;    //�� ����

    protected int shieldSpeed, level;  //�� �ӵ�, �÷��̾� ����
    protected float exp, maxExp, bulletTimer;   //���� ����ġ, �ִ� ������, ??

    public int HP { get; set; } //���� HP
    public int MaxHP { get; set; }  //�ִ� HP
    public float Speed { get; set; }    //�÷��̾� �ӵ�
    public int BulletHitMaxCount { get; set; }  //�Ѿ� ���� Ƚ��

    public float BulletFireDelayTime { get; set; }  //�Ѿ� ���� �ӵ�
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
        if (Ui.instance.gamestate != GameState.Play)    //GameState�� Play�� �ƴ϶��
            return;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            BulletFireDelayTime -= 0.1f;
        }
        x = Input.GetAxis("Horizontal");    //�÷��̾� �̵�(A, D)
        y = Input.GetAxis("Vertical");      //�÷��̾� �̵�(W, S)

        transform.Translate(new Vector3(x, y, 0f) * Time.deltaTime * Speed);

        if ((x == 0 && y == 0) && HP != 0)
        {
            animator.SetTrigger("idle");    //���ִ� �̹���
        }
        else if (HP != 0)
        {
            animator.SetTrigger("run");     //�޸��� �̹���
        }

        if (x != 0)
        {
            sr.flipX = x < 0 ? true : false;    //���� ���� ����(���׽�)
        }

        shieldParent.Rotate(Vector3.back * Time.deltaTime * shieldSpeed);   //shield ���������� ȸ��

        Monster[] monsters = FindObjectsOfType<Monster>();
        Box[] boxes = FindObjectsOfType<Box>();

        List<Monster> atkMonsterList = new List<Monster>();
        bulletTimer += Time.deltaTime;
        if (bulletTimer > BulletFireDelayTime)
        {
            //�ڽ��� �켱������ Ÿ��
            if (boxes.Length > 0)
            {
                //�ڽ��� ������� �ڽ� Ÿ��
                if (isShotDistanceBox(boxes))
                    BoxAttack(boxes);

                //�ڽ� �Ÿ��� �ְ�� ���͸� Ÿ��
                else
                    ShotDistanceAttackMonster(monsters);
            }

            else
                ShotDistanceAttackMonster(monsters);

            bulletTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.F1))   //ġƮŰ
        {
            BulletHitMaxCount++;
        }
    }

    public void Hit(int damage)    //damage = power(Monster) = 10;
    {
        if (Ui.instance.gamestate != GameState.Play)
            return;

        HP -= damage;   //HP ����
        Ui.instance.SetHP(HP, MaxHP);

        if (HP <= 0)    //�׾��� ��
        {
            Ui.instance.gamestate = GameState.Stop;

            animator.SetTrigger("dead");    //dead �ִϸ޴ϼ� ����
            Invoke("Dead", 2f); //2�� �� Dead �Լ� ����
        }
    }

    void Dead()
    {
        Ui.instance.ShowDeadPopup(level + 1);
    }

    void ShotDistanceAttackMonster(Monster[] monsters)
    {
        if (monsters.Length > 0)
        {
            float minDistance = 4f;
            Monster monster = null;
            foreach (Monster m in monsters)
            {
                float distance = Vector3.Distance(transform.position, m.transform.position);
                if (minDistance  > distance && m.hp > 0)
                {
                    minDistance = distance;
                    monster = m;
                }
            }

            //���� ����� ������ Ÿ��
            if (monster != null)
            {
                //Ÿ���� ã�� ���� ��ȯ
                Vector2 vec = transform.position - monster.transform.position;
                float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                firePos.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);

                BulletPool.Instance.Create(transform.position, firePos, 0);
                /*
                Bullet b = Instantiate(bullet, firePos);    //bullet ����
                b.SetHitMaxCount(BulletHitMaxCount + 1);
                b.transform.SetParent(null);
                */
            }

            /*
            //���� �ȿ� �ִ� ���� Ÿ��
            if(atkMonsterList.Count > 0)
            {
                Monster m = atkMonsterList[Random.Range(0, atkMonsterList.Count)];

                // Ÿ���� ã�� ���� ��ȯ
                Vector2 vec = transform.position - m.transform.position;
                float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                firePos.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);

                Bullet b = Instantiate(bullet, firePos);    //bullet ����
                b.SetHitMaxCount(BulletHitMaxCount + 1);
                b.transform.SetParent(null);
            }
            */
        }
    }

    bool isShotDistanceBox(Box[] boxs)
    {
        float minDistance = 5f;
        Box box = null;
        foreach (var item in boxs)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                box = item;
            }
        }

        return box == null ? false : true;
    }

    void BoxAttack(Box[] boxs)
    {
        float minDistance = 5f;
        Box box = null;
        foreach (var item in boxs)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance < minDistance) //item(box)���� �Ÿ��� 5f �̸��� ��
            {
                minDistance = distance;
                box = item;
            }
        }

        //Ÿ���� ã�� ���� ��ȯ
        Vector2 vec = transform.position - box.transform.position;
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        firePos.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);

        BulletPool.Instance.Create(transform.position, firePos, 0); //Bullet ����
        /*
        Bullet b = Instantiate(bullet, firePos);
        b.SetHitMaxCount(0);
        b.transform.SetParent(null);
        */
    }    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Item>())
        {
            collision.GetComponent<Item>().isPickup = true;     //isPickup�� true�� ����
        }
    }

    public void Shield()
    {
        float z = 360 / shieldCount;
        for (int i = 0; i < shieldCount; i++)
        {
            shields[i].rotation = Quaternion.Euler(0, 0, z * i);    //Quaternion.Euler : ������Ʈ�� ȸ����Ű�µ� ���
        }
    }

    public void AddShild()    //���� ����
    {
        shieldCount++;
        shields.Add(Instantiate(shildPrefab, shieldParent));    //shield ����
        Shield();
    }
}
