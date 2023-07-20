using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnController : MonoBehaviour
{
    [SerializeField] private Player p;

    [SerializeField] private Monster monster;
    [SerializeField] private Transform parent;

    [SerializeField] private BoxCollider2D[] boxColls;

    float spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CreateMonster", 1, 1);   //호출할 함수 이름, 첫 호출 시간, 반복 호출 간격
    }

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.play)
            return;

        if(spawnTimer > 3)
        {
            spawnTimer = 0;
            CreateMonster();
        }
    }

    void CreateMonster()
    {
        int rand = Random.Range(0, boxColls.Length);
        Vector2 v = RandomPosition(boxColls[rand]);    //랜덤 위치

        Monster m = Instantiate(monster, v, Quaternion.identity);
        m.SetPlayer(p);
        m.transform.SetParent(parent);
    }

    Vector2 RandomPosition(BoxCollider2D boxColl)
    {
        Vector2 pos = boxColl.transform.position;

        Vector2 range = new Vector2(boxColl.bounds.size.x, boxColl.bounds.size.y);

        range.x = Random.Range((range.x / 2) * -1, range.x / 2);    //boxColl 크기 지정(X)
        range.y = Random.Range((range.y / 2) * -1, range.y / 2);    //boxColl 크기 지정(Y)

        return pos + range;
    }
}
