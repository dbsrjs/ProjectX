using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum Type
    {
        Exp,
        Mag
    }

    public bool isPickup = false;
    public Transform target;    //target = player

    protected Type type = Type.Exp;
    protected float exp = 10;

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.Play)    //GameState�� Play�� �ƴ϶��
            return;

        if (isPickup)
        {
            float distance = Vector3.Distance(transform.position, target.position);    //�� ������ �Ÿ� ���
            transform.position = Vector3.Lerp(transform.position, target.position, (Time.deltaTime * distance) * 4f);    //target�� ù��° ��������� �̵�

            if(distance < 1f)
            {
                target.GetComponent<Player>().Exp += exp;   //����ġ ����
                Destroy(gameObject);    //UI ����
            }
        }
    }
}
