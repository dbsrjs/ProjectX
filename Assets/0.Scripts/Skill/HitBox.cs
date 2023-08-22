using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    Dictionary<string, Queue<HitEvent>> boxs = new Dictionary<string, Queue<HitEvent>>();
    static HitBox instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        instance.boxs.Clear();
    }

    public static HitEvent Get(HitEvent type,Transform pos)
    {
        string name = type.name;
        HitEvent hit = type;

        if (!instance.boxs.ContainsKey(name))
        {
            instance.boxs.Add(name, new Queue<HitEvent>());
        }
        Queue<HitEvent> hitBoxs = instance.boxs[name];

        if(hitBoxs.Count <= 0)
        {
            hitBoxs.Enqueue(Instantiate(type, pos));
        }
        hit = hitBoxs.Dequeue();
        hit.transform.rotation = pos.rotation;
        hit.transform.position = pos.position;
        hit.gameObject.SetActive(true);

        return hit;
    }

    public static void Remove(HitEvent hit)
    {
        string name = hit.name.Replace("(Clone)","");
        if (!instance.boxs.ContainsKey(name))
        {
            instance.boxs.Add(name, new Queue<HitEvent>());
        }

        hit.gameObject.SetActive(false);
        instance.boxs[name].Enqueue(hit);
    }
    
}
