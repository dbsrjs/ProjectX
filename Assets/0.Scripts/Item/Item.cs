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
        if (Ui.instance.gamestate != GameState.Play)    //GameState가 Play가 아니라면
            return;

        if (isPickup)
        {
            float distance = Vector3.Distance(transform.position, target.position);    //둘 사이의 거리 계산
            transform.position = Vector3.Lerp(transform.position, target.position, (Time.deltaTime * distance) * 4f);    //target이 첫번째 포디션으로 이동

            if(distance < 1f)
            {
                target.GetComponent<Player>().Exp += exp;   //경험치 증가
                Destroy(gameObject);    //UI 삭제
            }
        }
    }
}
