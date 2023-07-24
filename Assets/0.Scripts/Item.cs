using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Exp,
        Mag
    }

    public bool isPickup = false;
    public Transform target;    // target = Player

    protected Type type = Type.Exp;
    protected float exp = 10;

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.play)
            return;

        if (isPickup)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            transform.position = Vector3.Lerp(transform.position, target.position, (Time.deltaTime * distance) * 4f);

            if (distance < 1f)
            {
                target.GetComponent<Player>().Exp += exp;   //경험치 증가
                Destroy(gameObject);    //UI 삭제
            }
        }
    }
}
