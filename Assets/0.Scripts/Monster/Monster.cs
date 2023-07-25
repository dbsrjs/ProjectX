using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour   //abstract : �߻� Ŭ����
{
    private Player p;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] expPrefab;
    
    protected float atkTime;    //���� �ӵ�
    protected int power;
    protected int hp;

    private float atkTimer;

    private float hitFrezeTimer;    //�Ͻ� ����

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.play)
            return;

        if (p == null || hp <= 0)
            return;

        if (hitFrezeTimer > 0)     //0.5�� ���� ����
        {            
            hitFrezeTimer -= Time.deltaTime;
            return;
        }

        float x = p.transform.position.x - transform.position.x;

        sr.flipX = x < 0 ? true : x == 0 ? true : false;    //Ÿ��(�÷��̾�) ��ġ�� ���� ���� ���� ����

        float distance = Vector2.Distance(p.transform.position, transform.position);

        if (distance <= 1)   //����
        {
            atkTimer += Time.deltaTime;
            if (atkTimer > atkTime)
            {
                atkTimer = 0;
                p.Hit(power);
            }
        }
        else    //�̵�
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
        if (collision.GetComponent<Shield>())   //Shield(��)�� �浹
        {
            Dead(0.2f, 30);
        }

        else if(collision.GetComponent<Bullet>())    //�Ѿ˰� �浹
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
        if (hp <= 0)    //������
        {
            Destroy(GetComponent<Rigidbody2D>());    //Rigidbody2D ����
            GetComponent<CapsuleCollider2D>().enabled = false;  //CapsuleCollider2D OFF
            animator.SetTrigger("dead");
            StartCoroutine("CDropExp");
        }
    }

    IEnumerator CDropExp()
    {
        Ui.instance.KillCount++;
        yield return new WaitForSeconds(0.5f);    //0.5�� �ڿ�
        //����ġ ������ �����

        int rand = Random.Range(0, 101);    //����ġ ���� Ȯ��

        if(rand < 70)
            Instantiate(expPrefab[0], transform.position, Quaternion.identity);
        else if (rand < 90)
            Instantiate(expPrefab[1], transform.position, Quaternion.identity);
        else
            Instantiate(expPrefab[2], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);    //2�� �ڿ�
        Destroy(gameObject);    //���� ���� ����
    }
}
