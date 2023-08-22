using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Item_Box : Item
{
    public override void GetItem()
    {
        ItemPanal.Instance.Open();
    }

}
