using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Monster : Entity
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private string[] stats_set;
    [SerializeField] private HitEvent bullet;

    private float attack_Timer = 0;


    protected override void firstStats()
    {
        stats.Set("speed", 2);
        stats.Set("duration", 0.5f);
        stats.Set(stats_set);
    }

    protected override void Run()
    {
        Player player = GameManager.instance.player;
        float range = Vector2.Distance(player.transform.position, transform.position);
        
            attack_Timer += Time.deltaTime;
        if(range < stats_last.GetP("attack_range"))
        {
            if (stats_last.GetP("attack_speed") <= attack_Timer)
            {
                attack_Timer = 0;

                Quaternion rotate = new Quaternion(0, 0, 0, 0);

                HitEvent hit = HitBox.Get(bullet, gameObject.transform);

                hit.transform.SetParent(UI.Instance.bullets);

                try
                {
                    Transform en = GameManager.instance.player.transform;
                    Vector3 vec = en.position - hit.transform.position;
                    float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                    rotate = Quaternion.AngleAxis(angle-90, Vector3.forward);
                }
                catch { }

                hit.Damage = stats_last.GetP("damage");
                hit.Owner = this;
                hit.Remove(stats_last.GetP("duration"));
                hit.transform.rotation = rotate;
            }
        }
        else if (range < 30)
        {
            float x = player.transform.position.x - transform.position.x;
            sr.flipX = x < 0 ? true : false;
            
            Vector2 pos = player.transform.position - transform.position;

            transform.Translate(pos.normalized * Time.deltaTime * stats_last.Get("speed"));
        }
        else if (range > 60)
        {
            stats.Set("xp", -999);
            stats.Set("xp%", 1);
            Drop();
        }

        attack_Timer += Time.deltaTime;
    }

}
