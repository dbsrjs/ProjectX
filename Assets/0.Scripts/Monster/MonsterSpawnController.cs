using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnController : MonoBehaviour
{
    [SerializeField] private Monster[] monsters;
    [SerializeField] private Transform parent;

    [SerializeField] private MonsterSpawn[] monsterSpawns;
    void Start()
    {
        foreach (var mon in monsterSpawns)
        {
            mon.SetData(monsters, parent);
        }
    }
}
