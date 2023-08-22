using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] private List<Monster> monster;
    [SerializeField] private Transform monsters;
    [SerializeField] private RectTransform canvas;

    float spawnTimer = 1;
    float spawnITimer = 1;

    float h = 8;
    float w = 6;

    // Update is called once per frame
    void Update()
    {
        if (UI.Instance.gamestate != GameState.Play) return;
        if (spawnITimer < 0)
        {
            Vector3 loc = RandomLocal();

            //아이템 무작위 생성
            int rd = UnityEngine.Random.Range(0, 100);
            if (rd <= 2)
            {
                ItemManager.SpawnRandom(loc);
            }
            else if (rd <= 40)
            {
                ItemManager.SpawnExp(loc, 0.5f);
            }
            spawnITimer = 0.5f;
        }

        if (spawnTimer < 0)
        {
            int time = (int)UI.option.Get("time");

            if (time % 60 > 50)
            {
                if(time% 60 < 51) AudioManager.Instance.Play("wave");
                float timer = 1.5f - (time / 180)*0.1f;
                spawnTimer = Math.Max(timer * UnityEngine.Random.Range(0.5f, 1.5f),0.2f);
            }
            else
                spawnTimer = 4 * UnityEngine.Random.Range(0.5f, 1.5f);

            //소환 위치 지정

            Camera camera = Camera.main;
            w = Screen.width * 0.001f * (camera.orthographicSize * 2);
            h = Screen.height * 0.001f * (camera.orthographicSize * 2);

            Vector3 loc = RandomLocal();

            Monster mob = (Monster)MonsterManager.Get(monster[UnityEngine.Random.Range(0, monster.Count)], monsters);
            mob.transform.position = loc;
            float value = 1 + (time * (time / 60) * 0.00001f);

            mob.stats.Mul("speed", 1 + 1*(time*0.002f));
            mob.stats.Mul("max_hp", value * UnityEngine.Random.Range(0.8f, 1.2f));
            mob.stats.Set("hp", mob.stats.Get("max_hp"));

            mob.stats.Mul("xp", value);
            mob.stats.Mul("damage", value);
        }
        spawnITimer -= Time.deltaTime;
        spawnTimer -= Time.deltaTime;
    }

    Vector3 RandomLocal()
    {
        Transform player = GameManager.instance.player.transform;
        Vector2 spawnPos2 = new Vector2(player.position.x, player.position.y);
        Vector2 spawnPos = Vector2.zero;

        if (pm() == 1)
        {
            spawnPos.x += w * pm() * 0.75f;
            spawnPos.y += UnityEngine.Random.Range(0f, h + 1) * pm() * 0.75f;
        }
        else
        {
            spawnPos.x += UnityEngine.Random.Range(0f, w + 1) * pm() * 0.75f;
            spawnPos.y += h * pm() * 0.75f;
        }

        return spawnPos2 + spawnPos;
    }

    float pm()
    {
        if(UnityEngine.Random.Range(0,2) == 1)
        {
            return 1;
        }
        return -1;
    }

}
