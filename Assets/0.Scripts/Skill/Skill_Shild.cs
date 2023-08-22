using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Skill_Shild : Skill
{
    [SerializeField] Transform shlid_Object;
    float range;
    float size;

    protected override void Option()
    {
        stats.Set(new string[] { "range:1", "count:1", "size:1" });
        name = "½Çµå";
    }
    List<Transform> shlid = new List<Transform>();

    // Update is called once per frame

    protected override void PassiveSkill()
    {
        int count = (int)stats_last.GetP("count");

        if (count > shlid.Count)
        {
            shlid.Add(Instantiate(shlid_Object, transform));
            if (count == shlid.Count)
            {
                Shild_Rotate();
                Stats_Updata();
            }
        }
        if (count < shlid.Count)
        {
            Transform Shild = shlid[shlid.Count - 1];
            shlid.Remove(Shild);
            Destroy(Shild.gameObject);
            if (count == shlid.Count && count > 0)
            {
                Shild_Rotate();
                Stats_Updata();
            }
        }
        transform.Rotate(0, 0, Time.deltaTime*36 * (stats_last.GetP("speed")));
    }

    void Shild_Rotate()
    {
        if (shlid.Count <= 0) return;

        transform.localRotation = new Quaternion(0, 0, 0, 0);

        float rotate = 360 / shlid.Count;
        for (int i = 0; i < shlid.Count; i++)
        {
            shlid[i].localRotation = transform.localRotation;
            shlid[i].Rotate(0, 0, rotate * (i + 1));

            shlid[i].localPosition = shlid[i].up * stats_last.GetP("range");
            shlid[i].localScale = toSize(stats_last.GetP("size"));
        }
    }

    public override void Stats_Updata()
    {
        stats_last = stats.Copy();
        stats_last += GameManager.instance.player.stats_last;

        if (stats_last.GetP("range") != range || stats_last.GetP("size") != size)
        {
            Shild_Rotate();
        }
        range = stats_last.GetP("range");
        size = stats_last.GetP("size");

        for (int i = 0; i < shlid.Count; i++)
        {
            HitEvent hit = shlid[i].GetComponent<HitEvent>();
            hit.Owner = GameManager.instance.player;
            hit.HitCount = -1;
            hit.Damage = stats_last.GetP("damage");
        }
    }

    protected override void StatsUp(int lv)
    {
        if(lv > 1)
        {
            if(lv == 4)
            {
                stats.Add("speed%", 50);
            }
            else if(lv == 7)
            {
                stats.Add("speed%", 50);
            }
            else
            {
                stats.Add("count", 1);
            }

        }
    }
    public override void LastUpgard(int i)
    {
        if (i == 0)
        {
            stats.Add("count", 5);
            stats.Add("damage%", -20);
        }
        if (i == 1)
        {
            stats.Mul("speed%", 5);
        }
        if (i == 2)
        {
            stats.Set("damage%", stats.Get("count")*100);
            stats.Set("size%", stats.Get("count") * 100);
            stats.Set("speed%", 20);
            stats.Set("range", 3);
            stats.Set("count", 1);
        }
    }
}
