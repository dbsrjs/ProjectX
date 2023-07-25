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
        if (UI.instance.gamestate != GameState.Play)    //GameState�� Play�� �ƴ϶��
            return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer > 0.2f)    //spawnTimer�� 0.2f �̻��̶��
        {
            spawnTimer = 0;
            CrateMonster();    //�Լ� ����
        }
    }

    void CrateMonster()
    {
        int rand = Random.Range(0, boxColls.Length);
        Vector2 v = RandomPosition(boxColls[rand]);    //���� ��ġ

        int randSpawnCount = 0;
        if( monsters.Length > (UI.instance.KillCount / 10))    //������ ���� (���� ���� ���� 10)���� ũ�ٸ�
        {
            randSpawnCount = (UI.instance.KillCount / 10);
        }
        else
        {
            randSpawnCount = monsters.Length;   //randSpawnCount�� ������ ��
        }

        int mRand = Random.Range(0, randSpawnCount);
        Monster m = Instantiate(monsters[mRand], v, Quaternion.identity);   //���� mRand�� v�� ȸ������ �ʴ� ���µ� ����
        m.SetPlayer(p);    //Monster Player ����
        m.transform.SetParent(parent);     //Monster parget ����
    }

    Vector2 RandomPosition(BoxCollider2D boxColl)
    {
        Vector2 pos = boxColl.transform.position;

        Vector2 range = new Vector2(boxColl.bounds.size.x, boxColl.bounds.size.y);

        range.x = Random.Range((range.x / 2) * -1, range.x / 2);    //boxColl ũ�� ����(X)
        range.y = Random.Range((range.y / 2) * -1, range.y / 2);    //boxColl ũ�� ����(Y)

        return pos + range;
    }
}
