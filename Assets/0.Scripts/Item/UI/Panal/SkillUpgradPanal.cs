using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillUpgradPanal : Singleton<SkillUpgradPanal>
{
    [SerializeField] List<UpgradSUI> upgradSUIs;
    Skill skill;

    private void Awake()
    {
        var v = Instance;
        gameObject.SetActive(false);
    }

    public void Open(Skill skill)
    {
        int i = 0;
        this.skill = skill;
        UI.Instance.gamestate = GameState.Stop;
        gameObject.SetActive(true);


        foreach (var ui in upgradSUIs)
        {
            ui.gameObject.SetActive(false);
        }

        foreach(var txt in skill.getUpgrads())
        {
            UpgradSUI ui = upgradSUIs[i++];
            ui.gameObject.SetActive(true);
            ui.txt_Name.text = txt.Name;
            ui.txt_Info.text = txt.Lore;
        }
    }

    public void OnClick(int number)
    {
        skill.LastUpgard(number);
        skill.Stats_Updata();
        UI.Instance.gamestate = GameState.Play;
        gameObject.SetActive(false);
    }
}
