using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    Monster[] monsters;
    Transform parent;
    BoxCollider2D boxCollider;
    Player p;

    float spawnTimer;
    float spawnDelayTime;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Ui.instance.gamestate != GameState.Play)    //GameState�� Play�� �ƴ϶��
            return;

        if (p == null && GameManager.Insatnce != null)
        {
            p = GameManager.Insatnce.p;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnDelayTime)    //spawnTimer�� 0.2f �̻��̶��
        {
            spawnTimer = 0;
            CrateMonster();    //�Լ� ����

            spawnDelayTime = Random.Range(5f, 10f);
        }
    }

    public void SetData(Monster[] monsters, Transform parent)
    {
        this.monsters = monsters;
        this.parent = parent;
    }

    void CrateMonster()
    {
        Vector2 v = RandomPosition(boxCollider);    //���� ��ġ

        int randSpawnCount = 0;
        if (monsters.Length > (Ui.instance.KillCount / 10))    //������ ���� (���� ���� / 10)���� ũ�ٸ�
        {
            randSpawnCount = (Ui.instance.KillCount / 10);
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
