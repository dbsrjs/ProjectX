using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int HitCount { get; set; }
    public int HitMaxCount { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        HitCount = 0;
        Invoke("End", 2f);  //2초 후 End 함수 실행
        //Destroy(gameObject, 2f);    //2초 후에 삭제
    }

    // Update is called once per frame
    void Update()
    {
        if (Ui.Instance.gamestate != GameState.Play)    //GameState가 Play가 아니라면
            return;

        transform.Translate(Vector3.right * Time.deltaTime * 15f);    //오른쪽으로 회전
    }

    public void SetHitMaxCount(int count)
    {
        HitMaxCount = count;
    }

    public void End()
    {
        BulletPool.Instance.End(this);
    }
}
