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

    private List<Transform> shields = new List<Transform>();

    int hp, maxhp, shieldCount, shieldSpeed;
    float x, y;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxhp = 100;
        shieldSpeed = 40;
    }

    // Update is called once per frame
    void Update()
    {
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

        if(Input.GetKeyDown(KeyCode.F1))    //test
        {
            shieldCount++;
            shields.Add(Instantiate(ShieldPrefab, shieldParent));
            Shield();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            shieldCount--;
        }

        shieldParent.Rotate(Vector3.back * Time.deltaTime * shieldSpeed);   //bullet 오른쪽으로 회전
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
            collision.GetComponent<Item>().isPickup = true;
            collision.GetComponent<Item>().target = transform;
        }
    }
}
