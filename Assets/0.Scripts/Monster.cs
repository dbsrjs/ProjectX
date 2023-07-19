using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Player p;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject expPrefab;

    protected float atkTime = 2f;    //���� �ӵ�
    protected int power = 10;
    protected int hp = 50;

    private float atkTimer;

    private float hitFrezeTimer;    //�Ͻ� ����

    // tart is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if (collision.GetComponent<Shield>())
        {
            hitFrezeTimer = 0.5f;   //�Ͻ� ����
            hp -= 10;
            if (hp <= 0)    //������
            {
                Destroy(GetComponent<Rigidbody2D>());    //Rigidbody2D ����
                GetComponent<CapsuleCollider2D>().enabled = false;  //CapsuleCollider2D OF
                animator.SetTrigger("dead");
                //Invoke("DropExp", 1f);
                StartCoroutine("CDropExp");
            }
        }
    }

    IEnumerator CDropExp()
    {       
        yield return new WaitForSeconds(1f);    //1�� �ڿ�
        Instantiate(expPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);    //2�� �ڿ�
        Destroy(gameObject);    //���� ���� ����
    }
}
