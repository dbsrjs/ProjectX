using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Player : Entity
{
    [SerializeField] private SpriteRenderer sr;
    [HideInInspector] public List<Skill> skills;
    [HideInInspector] public Stats level = new Stats(new string[] { "lv:1", "exp:0", "exp_Max:5" });

    private Stats skillAddStats;

    protected override void firstStats() {
        stats.Set("get_range", 2f);
        stats.Set("speed", 3);
    }

    void Start()
    {
        StartOption();
    }
    public override Stats StatsUpdata()
    {
        stats_last = stats.Copy();
        stats_last += stats_Add.Copy();

        skillAddStats = new Stats();
        foreach (Skill s in skills)
        {
            if (s.skillType == SkillType.StatsUp)
            {
                skillAddStats += s.stats;
            }
        }
        stats_last += skillAddStats;

        foreach (Skill s in skills)
        {
            s.Stats_Updata();
        }
        return stats_last;
    }

    // Update is called once per frame
    protected override void Run()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");


        transform.Translate(new Vector2(x,y) * Time.deltaTime * stats_last.GetP("speed"));

        if(stats_last.GetP("hp") == 0)
        {
            animator.SetTrigger("Dead");
        }
        else if(x == 0 && y == 0)
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            animator.SetTrigger("Run");
            if (x != 0)
            {
                sr.flipX = x < 0 ? true : false;
            }
        }
    }

    public override void Death()
    {
        if (isAlive)
        {
            isAlive = false;
            animator.SetTrigger("Dead");
            Invoke("GameStop", 0.2f);
        }
    }

    public void GameStop()
    {
        UI.Instance.GameStop();
    }


    public virtual void StartOption()
    {

    }

    public virtual void LevelUpOption()
    {

    }
}
