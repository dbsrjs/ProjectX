using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum Type
    {
        Exp,
        Mag,
        Health,
        Boom
    }

    public bool isPickup = false;
    public Player p;

    protected Type type = Type.Exp;
    protected float exp = 10;

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.Play)    //GameState�� Play�� �ƴ϶��
            return;

        if (p == null)
        {
            p = GameManager.Insatnce.p;
        }
        
        if (isPickup == true)
        {
            Vector3 vec = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, p.transform.position, ref vec, Time.deltaTime * 10f);    //target�� ù��° ��������� �̵�
            float distance = Vector3.Distance(transform.position, p.transform.position);    //�� ������ �Ÿ� ���

            switch (type)
            {
                case Type.Exp:
                    Exp(distance);
                    break;
                case Type.Health:
                    Health();
                    break;
                case Type.Mag:
                    Magnet();
                    break;
                case Type.Boom:
                    break;
            }
        }
    }

    void Exp(float distance)
    {
        if (distance < 1f)
        {
            p.Exp += exp;
            Destroy(gameObject);    //UI ����
        }
    }

    void Health()
    {
        p.HP = p.MaxHP;
        p.Hit(0);
        Destroy(gameObject);
    }

    void Magnet()
    {
        Item[] items = FindObjectsOfType<Item>();
        foreach (var item in items)
        {
           item.isPickup = true;
        }
        Destroy(gameObject);
    }
}
