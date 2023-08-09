using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : Singleton<BulletPool>
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform parent;

    private Queue<Bullet> q = new Queue<Bullet>();

    public void Cler()
    {
        q.Clear();
    }


    public void Create(Vector3 startPos, Transform firePos, int count)
    {
        Bullet b = null;
        if(q.Count == 0)
        {
            b = Instantiate(bullet);    //bullet »ý¼º
            b.transform.SetParent(parent);
        }
        else
        {
            b = q.Dequeue();
            b.gameObject.SetActive(true);
        }
        b.transform.position = startPos;
        b.transform.localRotation = firePos.rotation;
        b.SetHitMaxCount(count);
    }

    public void End(Bullet b)
    {
        b.CancelInvoke("End");
        b.gameObject.SetActive(false);

        q.Enqueue(b);
    }
}
