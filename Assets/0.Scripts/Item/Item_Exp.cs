using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Item_Exp : Item
{
    [SerializeField] private List<Sprite> imgs;
    [SerializeField] private SpriteRenderer img;
    public float xp = 1;

    private void Start()
    {
        ImgSet();
    }
    public void ImgSet()
    {
        if (xp < 1)
        {
            img.sprite = imgs[0];
        }
        else if(xp < 10)
        {
            img.sprite = imgs[1];
        }
        else
        {
            img.sprite = imgs[2];
        }
    }

    public override void GetItem()
    {
        UI.Instance.AddExp(xp);
    }
}
