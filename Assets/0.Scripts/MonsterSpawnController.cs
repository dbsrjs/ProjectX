using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnController : MonoBehaviour
{
    [SerializeField] private Player p;

    [SerializeField] private Monster[] monsters;
    [SerializeField] private Transform parent;

    [SerializeField] private BoxCollider2D[] boxColls;


    float spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.instance.gamestate != GameState.Play)    //GameState가 Play가 아니라면
            return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer > 0.2f)    //spawnTimer가 0.2f 이상이라면
        {
            spawnTimer = 0;
            CrateMonster();    //함수 실행
        }
    }

    void CrateMonster()
    {
        int rand = Random.Range(0, boxColls.Length);
        Vector2 v = RandomPosition(boxColls[rand]);    //랜덤 위치

        int randSpawnCount = 0;
        if( monsters.Length > (UI.instance.KillCount / 10))    //몬스터의 수가 (죽은 몬스터 나누 10)보다 크다면
        {
            randSpawnCount = (UI.instance.KillCount / 10);
        }
        else
        {
            randSpawnCount = monsters.Length;   //randSpawnCount는 몬스터의 수
        }

        int mRand = Random.Range(0, randSpawnCount);
        Monster m = Instantiate(monsters[mRand], v, Quaternion.identity);   //몬스터 mRand을 v에 회전하지 않는 상태도 생성
        m.SetPlayer(p);    //Monster Player 지정
        m.transform.SetParent(parent);     //Monster parget 지정
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
