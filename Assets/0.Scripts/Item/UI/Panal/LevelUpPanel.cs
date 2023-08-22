using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : Singleton<LevelUpPanel>
{
    [SerializeField] List<Skill> skill_List;
    [SerializeField] List<UpgradUI> button_List;

    List<int> skills = new List<int>();

    void Awake()
    {
        var a = Instance;
        gameObject.SetActive(false);
    }

    public void Open()
    {
        skills.Clear();

        AudioManager.Instance.Play("levelup");
        int c = 0;
        GameManager.instance.player.stats.Set("nodamage", 2);
        List<int> ints = new List<int>();
        foreach(var ui in button_List)
        {
            ui.gameObject.SetActive(false);
        }

        for (int i=0; i< UI.option.GetP("panal");i++)
        {
            int rd;
            int k = 0;
            button_List[i].gameObject.SetActive(true);
            while (skill_List.Count > ints.Count)
            {
                rd = Random.Range(0, skill_List.Count);
                if (!ints.Contains(rd))
                {
                    ints.Add(rd);
                    if (skill_List[rd].option.Get("lv_max") > skill_List[rd].option.Get("lv"))
                    {
                        skills.Add(rd);
                        break;
                    }
                }
                k++;
                if (k > 1000) break;
            }
        }

        foreach (UpgradUI ui in button_List)
        {
            if (skills.Count <= c) break;
            Skill skill = skill_List[skills[c]];

            ui.txt_Name.text = skill.getName();
            ui.txt_Info.text = skill.getLore();
            ui.txt_Level.text = $"Lv. {skill.option.Get("lv")}";
            ui.img.sprite = skill.getIcon();
            c++;
        }

        gameObject.SetActive(true);
    }

    public void onUpgradeClick(int i)
    {
        GetSkill(skills[i]);
    }

    public void GetSkill(int i)
    {

        Skill skill = skill_List[i];

        if (skill.option.Get("lv") == 0)
        {
            skill_List[i] = Instantiate(skill, GameManager.instance.player.transform);
            skill_List[i].LevelUp();

            GameManager.instance.player.skills.Add(skill_List[i]);
        }
        else
        {
            skill.LevelUp();
        }

        UI.Instance.gamestate = GameState.Play;
        gameObject.SetActive(false);
        SkillInfos.instance.SkillSet();
    }
}
