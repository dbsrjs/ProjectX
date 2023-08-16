using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject[] items;

    public float Hp { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Hp = 30;
    }

    public void Hit(float damage)
    {
        Hp -= damage;
        AudioManager.instance.Play("hit1");    //hit 소리 실행
        if (Hp <= 0 && items.Length > 0)
        {
            int rand = Random.Range(0, items.Length);
            Instantiate(items[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>())
        {
            Hit(15);
            BulletPool.Instance.End(collision.GetComponent<Bullet>());
        }
    }
}
