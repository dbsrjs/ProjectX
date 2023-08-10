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
        Hp = 50;
    }

    public void Hit(float damage)
    {
        Hp -= damage;
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
