using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Skill_Sickle : Skill
{
    [SerializeField] HitEvent sickle;

    int up2 = -1;

    protected override void Option()
    {
        stats.Set(new string[] { "cooldown:1", "cooldown_max:8", "damage%:300", "size:3" });
        name = "³´";
    }

    // Update is called once per frame
    protected override void OnSkill()
    {
        if(up2 >= 0)
        {
            up2++;
            if(up2 > 9)
            {
                up2 = 0;
            }

            stats.Set("cooldown_max", 5.0f - (up2 * 0.5f));
            Stats_Updata();

        }

        AudioManager.Instance.Play("not");
        HitEvent bullet = HitBox.Get(sickle, transform);
        bullet.Owner = GameManager.instance.player;
        bullet.HitCount = -1;
        bullet.Damage = stats_last.GetP("damage");
        bullet.transform.localScale = toSize(stats_last.GetP("size"));
        bullet.Remove(0.5f);
    }

    protected override void StatsUp(int lv)
    {
        if(lv > 1)
        {
            if (lv == 3)
            {
                stats.Add("size%", 50);
            }
            else if (lv == 5)
            {
                stats.Set("cooldown_max%", 80);
            }
            else if (lv == 7)
            {
                stats.Add("damage%", 50);
            }
            else
            {
                stats.Add("size%", 20);
                stats.Add("damage%", 20);
            }
        }
    }

    public override void LastUpgard(int i)
    {
        if (i == 0)
        {
            stats.Mul("size%", 2);
            stats.Mul("cooldown_max%", 20);
            stats.Set("damage%", 9900);
        }
        if (i == 1)
        {
            up2 = 0;
            stats.Add("damage%", -20);
        }
    }
}
