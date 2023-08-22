using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float getRange = 0;

    // Update is called once per frame
    void Update()
    {
        if (UI.Instance.gamestate != GameState.Play) return;
        if (GameManager.instance.player == null) return;
        Player player = GameManager.instance.player;

        float range = Vector2.Distance(player.transform.position, transform.position);

        if (range < 0.3f + getRange)
        {
            GetItem();
            ItemManager.Remove(this);
        }
        else if(moveSpeed > 0 && range < player.stats.Get("get_range") + getRange)
        {

            Vector2 pos = player.transform.position - transform.position;
            transform.Translate(pos.normalized * Time.deltaTime * moveSpeed);
        } else if(range > 60)
        {
            ItemManager.Remove(this);
        }
    }

    public virtual void GetItem()
    {

    }
}
