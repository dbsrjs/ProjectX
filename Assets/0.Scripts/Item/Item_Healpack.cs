using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Item_Healpack : Item
{
    
    public override void GetItem()
    {
        Stats stats = GameManager.instance.player.stats;

        AudioManager.Instance.Play("heal");
        stats.Add("hp", stats.Get("max_hp") * 0.3f);
        if(stats.Get("hp") > stats.Get("max_hp"))
        {
            stats.Set("hp", stats.Get("max_hp"));
        }

    }

}
