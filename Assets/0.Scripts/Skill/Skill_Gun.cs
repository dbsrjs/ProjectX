using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Skill_Gun : Skill
{
    [SerializeField] HitEvent bullet_Object;
    Transform bullets;

    bool shootgun = false;

    protected override void Option()
    {
        stats.Set(new string[] { "hitcount:1", "speed%:10", "cooldown:1", "cooldown_max:2" , "bullet_speed:10", "duration:2" });
        name = "รั";
    }

    public void Start()
    {
        bullets = UI.Instance.bullets;
    }

    // Update is called once per frame
    protected override void OnSkill()
    {
        try {
            Transform en = MonsterManager.Instance.getTarget(transform.position);
            Vector3 vec = transform.position - en.position;
            float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
        } catch { }

        int bullet = 1;
        if (shootgun)
        {
            AudioManager.Instance.Play("shotgun");
            bullet = 8 + ((int)stats_last.GetP("hitcount") * 4);
        }
        else
        {

            AudioManager.Instance.Play("gun");
        }
        for (int i = 0; i < bullet; i++)
        {
            shoot();
        }
    }

    public void shoot()
    {
        HitEvent bullet = HitBox.Get(bullet_Object, transform);
        bullet.Remove(stats_last.GetP("duration"));
        bullet.transform.parent = bullets;

        int rt = -90;
        float speed = stats_last.GetP("bullet_speed");
        if (shootgun)
        {
            rt += Random.Range(-25, 25);
            bullet.transform.localScale = new Vector3(0.25f, 1f);
            speed *= Random.Range(1.4f, 0.7f);
        }
        else
        {
            bullet.transform.localScale = toSize(stats_last.GetP("size"));
        }

        bullet.transform.Rotate(new Vector3(0, 0, rt));
        bullet.Owner = GameManager.instance.player;
        bullet.HitCount = (int)stats_last.GetP("hitcount");
        bullet.Damage = stats_last.GetP("damage");

        bullet.GetComponent<Bullet>().speed = speed;
    }

    protected override void StatsUp(int lv)
    {
        if (lv == 2)
        {
            stats.Set("cooldown_max%", 70);
        }
        else if(lv > 2 && lv < 6)
        {
            stats.Add("cooldown_max%", -10);
            stats.Add("damage%", 10);
        }
        else if(lv >= 6)
        {
            stats.Add("damage%", 30);
        }
    }

    public override void LastUpgard(int i)
    {
        if (i == 0)
        {
            shootgun = true;
            stats.Set("duration", 0.3f);
            stats.Set("bullet_speed", 10);

            stats.Mul("cooldown_max%", 2);
            stats.Set("damage%", 50);
        }
        if (i == 1)
        {
            stats.Set("duration", 2);
            stats.Set("bullet_speed", 10);
            stats.Add("cooldown_max%", -30);
            stats.Mul("damage%", 1.35f);
        }
        if (i == 2)
        {
            stats.Set("duration", 4);
            stats.Set("bullet_speed", 2);
            stats.Mul("cooldown_max%", 5);
            stats.Mul("damage%", 10);
            stats.Mul("size%", 10);
            stats.Set("hitcount", -1);
        }
    }
}
