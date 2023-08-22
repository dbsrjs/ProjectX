using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum BulletType{
    Forward,
    Targeting,
    Homing
}

public class Bullet : MonoBehaviour
{
    public BulletType bullettype = BulletType.Forward;
    public Entity target;
    public float speed = 3;

    // Update is called once per frame
    void Update()
    {
        if (UI.Instance.gamestate != GameState.Play) return;

        if (bullettype == BulletType.Forward)
        {
            transform.position += transform.up * Time.deltaTime * speed;
        }

        if (bullettype == BulletType.Targeting && target != null)
        {
            Vector2 pos = target.transform.position - transform.position;
            transform.Translate(pos.normalized * Time.deltaTime * speed);
        }

        if (bullettype == BulletType.Homing)
        {
            Transform target =  MonsterManager.Instance.getTarget(transform.position);
            if (target != null)
            {
                Vector2 pos = target.position - transform.position;
                transform.Translate(pos.normalized * Time.deltaTime * speed);
            }
        }
    }
}
