using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnController : MonoBehaviour
{
    [SerializeField] private Monster[] monsters;
    [SerializeField] private Transform parent;

    [SerializeField] private MonsterSpawn[] monsterSpawns;

    [SerializeField] private Box box;

    void Start()
    {
        foreach (var mon in monsterSpawns)
        {
            mon.SetData(monsters, parent);
        }

        InvokeRepeating("SpawnBox", 2, 6);  //SpawnBox�� 2�� �ڿ� 6�� ���� ����
    }

    void SpawnBox()
    {
        int rand = Random.Range(0, monsterSpawns.Length);
        monsterSpawns[rand].SetBox(box);
    }
}
