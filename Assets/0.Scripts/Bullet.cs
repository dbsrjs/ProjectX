using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int HitCount
    {
        get; set;
    }

    public int HitMaxCount
    {
        get; set;
    }

    // Start is called before the first frame update
    void Start()
    {
        HitCount = 0;
        HitMaxCount = 1;
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.play)
            return;

        transform.Translate(Vector3.right * Time.deltaTime * 15f);  //오른쪽으로 회전
    }

    public void SetHitMaxCount(int count)
    {
        HitMaxCount = count;
    }
}
