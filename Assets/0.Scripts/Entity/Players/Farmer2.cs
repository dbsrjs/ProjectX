using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer2 : Player
{
    public override void StartOption()
    {
        stats.Add("speed", 1);
        UI.option.Set("panal", 4);
    }

    public override void LevelUpOption()
    {

    }
}
