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

    protected int shieldSpeed, level;  //삽 속도, 플레이어 레벨
    protected float exp, maxExp, bulletTimer;   //현재 경험치, 최대 경험지, ??

    public int HP { get; set; } //현재 HP
    public int MaxHP { get; set; }  //최대 HP
    public float Speed { get; set; }    //플레이어 속도
    public int BulletHitMaxCount { get; set; }  //총알 관통 횟수

    public float BulletFireDelayTime { get; set; }  //총알 연사 속도
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

        if (Input.GetKeyDown(KeyCode.F1))
        {
            BulletFireDelayTime -= 0.1f;
        }
        x = Input.GetAxis("Horizontal");    //플레이어 이동(A, D)
        y = Input.GetAxis("Vertical");      //플레이어 이동(W, S)

        transform.Translate(new Vector3(x, y, 0f) * Time.deltaTime * Speed);

        if ((x == 0 && y == 0) && HP != 0)
        {
            animator.SetTrigger("idle");    //서있는 이미지
        }
        else if (HP != 0)
        {
            animator.SetTrigger("run");     //달리는 이미지
        }

        if (x != 0)
        {
            sr.flipX = x < 0 ? true : false;    //보는 방향 변경(삼항식)
        }

        shieldParent.Rotate(Vector3.back * Time.deltaTime * shieldSpeed);   //shield 오른쪽으로 회전

        Monster[] monsters = FindObjectsOfType<Monster>();
        Box[] boxes = FindObjectsOfType<Box>();

        List<Monster> atkMonsterList = new List<Monster>();
        bulletTimer += Time.deltaTime;
        if (bulletTimer > BulletFireDelayTime)
        {
            //박스를 우선적으로 타격
            if (boxes.Length > 0)
            {
                //박스가 있을경우 박스 타격
                if (isShotDistanceBox(boxes))
                    BoxAttack(boxes);

                //박스 거리가 멀경우 몬스터를 타격
                else
                    ShotDistanceAttackMonster(monsters);
            }

            else
                ShotDistanceAttackMonster(monsters);

            bulletTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.F1))   //치트키
        {
            BulletHitMaxCount++;
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
            Ui.instance.gamestate = GameState.Stop;

            animator.SetTrigger("dead");    //dead 애니메니션 실행
            Invoke("Dead", 2f); //2초 후 Dead 함수 실행
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

            //나와 가까운 적부터 타격
            if (monster != null)
            {
                //타겟을 찾아 방향 전환
                Vector2 vec = transform.position - monster.transform.position;
                float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                firePos.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);

                BulletPool.Instance.Create(transform.position, firePos, 0);
                /*
                Bullet b = Instantiate(bullet, firePos);    //bullet 생성
                b.SetHitMaxCount(BulletHitMaxCount + 1);
                b.transform.SetParent(null);
                */
            }

            /*
            //범위 안에 있는 적을 타격
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
            if (distance < minDistance) //item(box)와의 거리가 5f 미만일 때
            {
                minDistance = distance;
                box = item;
            }
        }

        //타겟을 찾아 방향 전환
        Vector2 vec = transform.position - box.transform.position;
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        firePos.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);

        BulletPool.Instance.Create(transform.position, firePos, 0); //Bullet 생성
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
            collision.GetComponent<Item>().isPickup = true;     //isPickup을 true로 변경
        }
    }

    public void Shield()
    {
        float z = 360 / shieldCount;
        for (int i = 0; i < shieldCount; i++)
        {
            shields[i].rotation = Quaternion.Euler(0, 0, z * i);    //Quaternion.Euler : 오브젝트를 회전시키는데 사용
        }
    }

    public void AddShild()    //방패 증가
    {
        shieldCount++;
        shields.Add(Instantiate(shildPrefab, shieldParent));    //shield 생성
        Shield();
    }
}
