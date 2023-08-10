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

        InvokeRepeating("SpawnBox", 2, 6);  //SpawnBox를 2초 뒤에 6초 마다 실행
    }

    void SpawnBox()
    {
        int rand = Random.Range(0, monsterSpawns.Length);
        monsterSpawns[rand].SetBox(box);
    }
}
