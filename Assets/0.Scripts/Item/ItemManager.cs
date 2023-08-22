using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private List<Item> items;
    [SerializeField] private Item_Exp Exp;

    Dictionary<string, Queue<Item>> boxs = new Dictionary<string, Queue<Item>>();

    static public void Spawn(int code, Vector3 pos)
    {

        Get(Instance.items[code], pos).transform.SetParent(Instance.transform);
    }
    static public void SpawnRandom(Vector3 pos)
    {

        Get(Instance.items[Random.Range(0, Instance.items.Count)], pos).transform.SetParent(Instance.transform);
    }

    static public void SpawnExp(Vector3 pos,float xp)
    {
        Item_Exp exp = (Item_Exp)Get(Instance.Exp, pos);
        exp.xp = xp;
        exp.ImgSet();
        exp.transform.SetParent(Instance.transform);
    }

    public static Item Get(Item type,Vector3 pos)
    {
        string name = type.name;
        Item hit = type;

        if (!Instance.boxs.ContainsKey(name))
        {
            Instance.boxs.Add(name, new Queue<Item>());
        }
        Queue<Item> hitBoxs = Instance.boxs[name];

        if (hitBoxs.Count <= 0)
        {
            hitBoxs.Enqueue(Instantiate(type, pos, Quaternion.identity));
        }
        hit = hitBoxs.Dequeue();
        hit.transform.position = pos;
        hit.gameObject.SetActive(true);

        return hit;
    }

    public static void Remove(Item hit)
    {
        string name = hit.name.Replace("(Clone)", "");
        if (!Instance.boxs.ContainsKey(name))
        {
            Instance.boxs.Add(name, new Queue<Item>());
        }

        hit.gameObject.SetActive(false);
        Instance.boxs[name].Enqueue(hit);
    }

}
