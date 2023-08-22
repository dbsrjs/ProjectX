using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [HideInInspector] public Stats stats_last = new Stats();
    [HideInInspector] public Stats stats_Add = new Stats();
    [HideInInspector] public Stats stats = new Stats(new string[] { "hp:2", "max_hp:2", "speed:3", "attack_speed:1", "damage:0.5", "attack_range:1", "xp:1" });
    [SerializeField] protected Animator animator;

    protected bool isAlive = true;

    private void Awake()
    {
        firstStats();
        StatsUpdata();
    }

    protected virtual void firstStats()
    {

    }

    public bool IsAttack()
    {
        return isAlive && stats_last.Get("nodamage") <= 0;
    }

    private void Update()
    {
        if (UI.Instance.gamestate != GameState.Play) return;
        if (GameManager.instance.player == null || !isAlive) return;
        if (stats.isRep || stats_Add.isRep)
        {
            stats.isRep = false;
            stats_Add.isRep = false;
            StatsUpdata();
        }
        if (stats_last.Get("nodamage") > 0) stats_last.Add("nodamage", -Time.deltaTime);
        Run();
    }

    protected virtual void Run()
    {

    }

    public void Damage(float damage,Entity target)
    {
        UI.SpawnText(transform.position, $"{Mathf.Round(damage*10.0f)*0.1f}", 1, new Vector3(0, 1f, 0));
        animator.SetTrigger("Hit");
        stats.Add("hp", -damage);
        if(stats.Get("hp") <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        if (isAlive)
        {
            UI.Instance.KillCount += 1;
            isAlive = false;
            animator.SetTrigger("Dead");
            Invoke("Drop", 1f);
        }
    }

    public virtual void Drop()
    {
        MonsterManager.Remove(this);
        int xp = (int)stats.GetP("xp");

        if (xp > 0)
        {
            ItemManager.SpawnExp(transform.position, xp);
        }
    }

    public virtual Stats StatsUpdata()
    {
        stats_last = stats.Copy();
        stats_last += stats_Add.Copy();
        return stats_last;
    }

    public bool Alive {
        get { return isAlive; }
        set { isAlive = value; }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        HitEvent hit = collision.GetComponent<HitEvent>();

        if (hit && hit.Owner.tag != gameObject.tag && hit.isAttack(this))
        {
            Damage(hit.Damage, hit.Owner);
        }
    }
}
