using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Item_Meg : Item
{
    
    public override void GetItem()
    {

        AudioManager.Instance.Play("meg");
        GameManager.instance.player.stats.Add("get_range", 50);
        Invoke("rangeBack", 3f);

    }

    void rangeBack()
    {
        GameManager.instance.player.stats.Add("get_range", -50);
    }
}
