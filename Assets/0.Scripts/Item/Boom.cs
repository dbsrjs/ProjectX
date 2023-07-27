using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    private float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    public IEnumerator OnTriggerStay2D(Collider2D collision)
    {
        yield return new WaitForSeconds(3f);    //3초 후
        Monster monster = GameObject.FindObjectOfType<Monster>();   //Monster 스크렙트를 갖고 있는 gameObject 찾기
        monster.Dead(0f, 200);    //Dead 함수 호출(킬은 올라 가지만 실질적으론 한마리만 죽음)
        Debug.Log("test");
        Destroy(gameObject);
    }
}
