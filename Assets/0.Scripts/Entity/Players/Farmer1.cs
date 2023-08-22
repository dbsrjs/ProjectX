using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer1 : Player
{
    public override void StartOption()
    {
        LevelUpPanel.Instance.GetSkill(0);
        GameManager.instance.player.skills[0].stats.Add("hitcount",1);
    }

    public override void LevelUpOption()
    {

    }
}
