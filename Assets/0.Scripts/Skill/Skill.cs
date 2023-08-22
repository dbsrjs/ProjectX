using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    Passive,
    Active,
    StatsUp
}
[System.Serializable]
public class SkillUpgrad
{
    public string Name;
    public string Lore;
}

public class Skill : MonoBehaviour
{
    [SerializeField] protected Sprite icon;
    [SerializeField] protected string Name = "¿Ã∏ß";
    [SerializeField] protected List<string> Lores;
    [SerializeField] public List<SkillUpgrad> skillUpgrads;

    public Stats option = new Stats(new string[] { "lv:0", "lv_max:8" });

    public Stats stats = new Stats(new string[] { "count:1", "hitcount:-1", "speed%:100", "damage%:100", "size%:100", "size:1", "cooldown:0", "cooldown_max:0" });
    protected Stats stats_last;
    public SkillType skillType = SkillType.Active;

    public void Awake()
    {
        Option();
        Update();
    }

    public void Start()
    {
        Stats_Updata();
    }

    public bool LevelUp()
    {
        int lv_max = (int)option.GetP("lv_max");
        int lv = (int)option.GetP("lv");
        if (lv_max > lv)
        {
            option.Add("lv", 1);
            if (lv_max == lv + 1 && skillUpgrads.Count > 0)
            {
                SkillUpgradPanal.Instance.Open(this);
            }
            else
            {
                StatsUp(lv + 1);
                Stats_Updata();
            }
            return true;
        }
        return false;
    }

    void Update()
    {
        if (UI.Instance.gamestate != GameState.Play) return;
        if (option.Get("lv") <= 0) return;
        if (skillType == SkillType.StatsUp) return;

        if (stats_last.Get("cooldown") < 0)
        {
            stats_last.Set("cooldown", stats_last.GetP("cooldown_max"));
            OnSkill();
        }
        PassiveSkill();
        stats_last.Add("cooldown", -Time.deltaTime);
    }

    public virtual void Stats_Updata() {
        float cd = 0;
        if (skillType == SkillType.StatsUp)
            return;

        if (stats_last != null)
            cd = stats_last.Get("cooldown");

        stats_last = stats.Copy();
        stats_last += GameManager.instance.player.stats_last;
        
        if(cd != 0) stats_last.Set("cooldown", cd);
    }

    protected Vector2 toSize(float size)
    {
        return new Vector2(size, size);
    }

    protected virtual void OnSkill() { }
    protected virtual void PassiveSkill() { }
    protected virtual void Option() { }
    public virtual void LastUpgard(int i) { }

    public Sprite getIcon() { return icon; }
    public string getName() { return Name; }
    public string getLore() { return Lores[Mathf.Min((int)option.Get("lv"), Lores.Count - 1)]; }
    protected virtual void StatsUp(int lv) { }
    public List<SkillUpgrad> getUpgrads() { return skillUpgrads; }

}
