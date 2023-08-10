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
        Invoke("End", 2f);  //2�� �� End �Լ� ����
        //Destroy(gameObject, 2f);    //2�� �Ŀ� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (Ui.Instance.gamestate != GameState.Play)    //GameState�� Play�� �ƴ϶��
            return;

        transform.Translate(Vector3.right * Time.deltaTime * 15f);    //���������� ȸ��
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
