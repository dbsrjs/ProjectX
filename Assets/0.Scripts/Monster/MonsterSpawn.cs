using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    Monster[] monsters;
    Transform parent;
    BoxCollider2D boxCollider;
    Player p;

    Box box;

    float spawnTimer;
    float spawnDelayTime;

    float time;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Ui.Instance.gamestate != GameState.Play)    //GameState가 Play가 아니라면
            return;

        if (p == null && GameManager.Insatnce != null)
        {
            p = GameManager.Insatnce.p;
        }

        spawnTimer += Time.deltaTime;
        time += Time.deltaTime;

        if (spawnTimer > spawnDelayTime)    //spawnTimer가 0.2f 이상이라면
        {
            spawnTimer = 0;
            CrateMonster();    //함수 실행

            spawnDelayTime = Random.Range(0.8f, 2f);
        }

        if (box != null)
        {
            Vector2 v = RandomPosition(boxCollider);    //랜덤 위치

            Box b = Instantiate(box, v, Quaternion.identity);   //box를 v(랜덤 위치)에 회전하지 않는 상태도 생성
            b.transform.SetParent(null);     //Monster parget 지정

            box = null;
        }
    }

    public void SetData(Monster[] monsters, Transform parent)
    {
        this.monsters = monsters;
        this.parent = parent;
        spawnDelayTime = 0f;
        spawnTimer = float.MaxValue;
    }

    void CrateMonster()
    {
        Vector2 v = RandomPosition(boxCollider);    //랜덤 위치

        int randSpawnCount = 0;
        if (monsters.Length > (Ui.Instance.KillCount / 10))    //몬스터의 수가 (죽은 몬스터 / 10)보다 크다면
        {
            randSpawnCount = (Ui.Instance.KillCount / 10);
        }
        else
        {
            randSpawnCount = monsters.Length;   //randSpawnCount는 몬스터의 수
        }

        int mRand = Random.Range(0, randSpawnCount);
        Monster m = Instantiate(monsters[mRand], v, Quaternion.identity);   //몬스터 mRand을 v(랜덤 위치)에 회전하지 않는 상태도 생성
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

    private void Time30s()  //test
    {
        for (int i = 0; i < 6; i++)
        {
            CrateMonster();
        }
    }

    public void SetBox(Box box)
    {
        this.box = box;
    }

    /// <summary>
    /// 총알이 벽에 충돌 했을 때 사용이 끝난다고 판단
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>())
        {
            collision.GetComponent<Bullet>().End();
        }
    }
}
