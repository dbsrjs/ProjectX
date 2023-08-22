using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    [SerializeField] SpriteRenderer Hp_bar;
    [SerializeField] Entity unit;

    // Update is called once per frame
    void Update()
    {
        float hpp = unit.stats.Get("hp") / unit.stats.GetP("max_hp");

        Hp_bar.size = new Vector2(Mathf.Lerp(0,1,hpp), 0.25f);
    }
}
