using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    [SerializeField] Entity owner;
    float damage = 0;
    float unit_delay = 0.5f;
    float attack_delay = 0;

    float attack_time = 0;
    int hitcount = -1;

    Dictionary<Entity,float> delay = new Dictionary<Entity, float>();

    // Update is called once per frame

    void Update()
    {
        foreach (Entity entity in new List<Entity>(delay.Keys))
        {
            delay[entity] -= Time.deltaTime;
        }
        attack_time -= Time.deltaTime;


        if (Vector2.Distance(GameManager.instance.player.transform.position, transform.position) > 70)
        {
            End();
        }
    }

    public Entity Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    public float Damage{
        get { return damage;}
        set { damage = value; }
    }
    public int HitCount
    {
        get { return hitcount; }
        set { hitcount = value; }
    }

    public void Remove(float time)
    {
        Invoke("End", time);
    }

    public void End()
    {
        CancelInvoke("End");
        HitBox.Remove(this);
        delay.Clear();
    }

    public void SetUnitDelay(float time)
    {
        unit_delay = time;
    }
    public void SetAttackDelay(float time)
    {
        attack_delay = time;
    }
    public Boolean isAttack(Entity entity)
    {
        if (!entity.IsAttack()) return false;

        if (attack_time <= 0)
        {
            attack_time = attack_delay;
            if (hitcount != 0)
            {
                try
                {
                    if (delay[entity] > 0)
                    {
                        return false;
                    }
                }
                catch { }
                hitcount--;
                delay[entity] = unit_delay;
                if (hitcount == 0) End();
                return true;
            }
        }
        return false;
    }
}
