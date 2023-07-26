using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour   //abstract : �߻� Ŭ����
{
    private Player p;
    private Boom boom;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] expPrefab;

    protected float atkTime;    //���� �ӵ�
    protected int power;    //���ݷ�
    protected int hp;   //ä��

    private float atkTimer;

    private float hitFrezeTimer;    //�Ͻ� ����

    // Update is called once per frame

    private void Start()
    {
        boom.GetComponent<Boom>().target = transform;
    }

    void Update()
    {
        if (UI.instance.gamestate != GameState.Play)    //GameState�� Play�� �ƴ϶��
            return;

        if (p == null || hp <= 0)   //�÷��̾ ���ų� �׾��� ��
            return;

        if(hitFrezeTimer > 0)   //�Ͻ� ���� �ð��� 0 �̻��̶��
        {
            hitFrezeTimer -= Time.deltaTime;
            animator.SetTrigger("hit");    //hit �ִϸ��̼� ����
            return;
        }
        else
        {
            animator.SetTrigger("run");    //run �ִϸ��̼� ����
        }

        float x = p.transform.position.x - transform.position.x;

        sr.flipX = x < 0 ? true : x == 0 ? true : false;    //Ÿ��(�÷��̾�) ��ġ�� ���� ���� ���� ����, ���׽�

        float distance = Vector2.Distance(p.transform.position, transform.position);    //�� ������ �Ÿ� ���
        
        if (distance <= 1)  //�Ÿ��� 1 ���϶�� 
        {
            atkTimer += Time.deltaTime;
            //����
            if(atkTimer > atkTime)
            {
                atkTimer = 0;
                p.Hit(power);
            }
        }
        else
        {
            //�̵�
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
        if(collision.GetComponent<Shild>())   //Shield(��)�� �浹
        {
            Dead(0.2f, 30);
        }
        else if(collision.GetComponent<Bullet>())    //�Ѿ˰� �浹
        {
            collision.GetComponent<Bullet>().HitCount++;    //HitCount ����
            if (collision.GetComponent<Bullet>().HitCount >= collision.GetComponent<Bullet>().HitMaxCount)  //HitCount�� HitMaxCount���� ũ�ٸ�
            {
                Destroy(collision.gameObject);     //Bullet ����
            }
            Dead(0.5f, 20);
        }
    }

    public void Dead(float frezeTime, int damage)
    {
        hitFrezeTimer = frezeTime;  //hitFrezeTimer �� ����
        hp -= damage;   //�������� ����ؼ� hp ����
        animator.SetTrigger("hit");    //hit �ִϸ��̼� ����
        AudioManager.instance.Play("hit1");    //hit �Ҹ�

        if (hp <= 0)    //������
        {
            Destroy(GetComponent<Rigidbody2D>());   //Rigidbody2D ����
            GetComponent<CapsuleCollider2D>().enabled = false;  //CapsuleCollider2D OFF
            animator.SetTrigger("dead");    //dead �ִϸ��̼� ����
            StartCoroutine("CDropExp");    //CDropExp �ڷ�ƾ ����
        }
    }

    IEnumerator CDropExp()
    {
        UI.instance.KillCount++;    //KillCount Text ����
        yield return new WaitForSeconds(1f);    //1�� ��
        // ����ġ ������ �����
        // TODO : ����ġ ���� Ȯ���� ���� �ؾ���...
        int rand = Random.Range(0, 101);    //0~100

        if(rand < 70)
            Instantiate(expPrefab[0], transform.position, Quaternion.identity);
        else if (rand < 90)
            Instantiate(expPrefab[1], transform.position, Quaternion.identity);
        else
            Instantiate(expPrefab[2], transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);    //2�� ��
        Destroy(gameObject);    //���� ����
    }
}
