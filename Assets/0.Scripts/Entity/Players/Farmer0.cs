using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer0 : Player
{
    public override void StartOption()
    {
        Vector3 vtr = gameObject.transform.position;
        ItemManager.SpawnExp(vtr - gameObject.transform.up * 3, level.Get("exp_Max") * 5);
    }

    public override void LevelUpOption()
    {

    }
}
