using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    Dictionary<string, Queue<Entity>> boxs = new Dictionary<string, Queue<Entity>>();

    public Transform getTarget(Vector2 loc)
    {
        Transform target = null;
        float min = 99999;

        for (int i=0; i<transform.childCount ;i++)
        {
            Transform child = transform.GetChild(i);
            float range = Vector2.Distance(child.position, loc);
            if(range  < min)
            {
                if (child.GetComponent<Entity>().Alive)
                {
                    min = range;
                    target = child;
                }
            }
        }
        if(target != null)
        {
            return target;
        }
        return null;
    }


    public static Entity Get(Entity type, Transform pos)
    {
        string name = type.name;
        Entity hit = type;

        if (!Instance.boxs.ContainsKey(name))
        {
            Instance.boxs.Add(name, new Queue<Entity>());
        }
        Queue<Entity> hitBoxs = Instance.boxs[name];

        if (hitBoxs.Count <= 0)
        {
            hitBoxs.Enqueue(Instantiate(type, pos));
        }
        hit = hitBoxs.Dequeue();
        hit.transform.position = pos.position;
        hit.gameObject.SetActive(true);
        hit.Alive = true;

        return hit;
    }

    public static void Remove(Entity hit)
    {
        string name = hit.name.Replace("(Clone)", "");
        if (!Instance.boxs.ContainsKey(name))
        {
            Instance.boxs.Add(name, new Queue<Entity>());
        }

        hit.gameObject.SetActive(false);
        Instance.boxs[name].Enqueue(hit);
    }

}
