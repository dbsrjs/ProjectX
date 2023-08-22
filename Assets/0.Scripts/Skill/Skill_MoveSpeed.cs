using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Skill_MoveSpeed : Skill
{

    protected override void Option()
    {
        option.Set("lv_max",5);
        stats.Set(new string[] { "cooldown_max:10" });
    }

    protected override void OnSkill()
    {
        Stats s = GameManager.instance.player.stats;

        AudioManager.Instance.Play("heal");
        s.Add("hp", s.Get("max_hp") * 0.1f);
        if(s.Get("hp") > s.Get("max_hp"))
        {
            s.Set("hp", s.Get("max_hp"));
        }
    }

    protected override void StatsUp(int lv)
    {
        if (lv > 1)
        {
            stats.Add("cooldown_max", -1f);
        }
    }
}
