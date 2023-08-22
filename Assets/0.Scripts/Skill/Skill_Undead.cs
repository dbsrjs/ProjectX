using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Skill_Undead : Skill
{
    [SerializeField] HitEvent bullet_Object;
    [SerializeField] HitEvent effect_Object;
    Transform bullets;

    protected override void Option()
    {
        stats.Set(new string[] { "hitcount:-1", "count:1","size:0.5", "speed%:50", "cooldown:0", "cooldown_max:10", "damage%:30", "duration:3" });
        name = "รั";
    }

    new public void Start()
    {
        bullets = UI.Instance.bullets;
    }

    // Update is called once per frame
    protected override void OnSkill()
    {
        AudioManager.Instance.Play("zombie");
        for (int i=0; i< stats_last.GetP("count");i++) {
            Invoke("spawn", i * 0.05f);
        }
    }

    void spawn()
    {
        Vector3 pos = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 1);

        HitEvent effect = HitBox.Get(effect_Object, transform);
        effect.Remove(0.5f);
        effect.transform.localPosition = pos;
        effect.transform.parent = bullets;
        effect.Owner = GameManager.instance.player;
        effect.transform.localScale = toSize(stats_last.GetP("size")*1.25f);

        HitEvent bullet = HitBox.Get(bullet_Object, transform);
        bullet.transform.localPosition = pos;
        bullet.SetAttackDelay(0.2f);
        bullet.SetUnitDelay(0);

        bullet.transform.parent = bullets;
        bullet.Owner = GameManager.instance.player;
        bullet.HitCount = (int)stats_last.GetP("hitcount");
        bullet.Damage = stats_last.GetP("damage");
        bullet.transform.localScale = toSize(stats_last.GetP("size"));
        bullet.Remove(stats_last.GetP("duration"));

        bullet.GetComponent<Bullet>().speed = stats_last.GetP("speed") * Random.Range(0.9f,1.1f);
    }

    protected override void StatsUp(int lv)
    {
        if(lv > 1)
        {
            if(lv == 4 || lv == 7)
            {
                stats.Add("count", 1);
            }
            else if (lv == 6)
            {

                stats.Add("speed%", 50);
            }
            else
            {
                stats.Add("duration%", 25);
            }

        }
    }

    public override void LastUpgard(int i)
    {
        if (i == 0)
        {
            stats.Set("count", 1);
            stats.Mul("damage%", 3);
            stats.Mul("size%", 3);
            stats.Add("duration%", 100);
        }
        if (i == 1)
        {
            stats.Add("count", 5);
            stats.Mul("size%", 0.7f);
            stats.Mul("speed%", 3);
            stats.Mul("duration%", 0.4f);
        }
        if (i == 2)
        {
            stats.Add("cooldown_max%", -30);
            stats.Mul("damage%", 10);
            stats.Set("hitcount", 1);
        }
    }
}
