using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfos : MonoBehaviour
{
    public static SkillInfos instance;

    [SerializeField] SkillInfo skillGui;
    private Dictionary<Skill, SkillInfo> skillInfo = new Dictionary<Skill, SkillInfo>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SkillSet()
    {
        foreach (Skill skill in GameManager.instance.player.skills)
        {
            if (!skillInfo.ContainsKey(skill))
            {
                SkillInfo s = Instantiate(skillGui, transform);
                s.gameObject.SetActive(true);
                skillInfo.Add(skill, s);
            }
            skillInfo[skill].SetUI(skill.getIcon() ,(int)skill.option.Get("lv"), (int)skill.option.Get("lv_max"));
        }
    }
}
