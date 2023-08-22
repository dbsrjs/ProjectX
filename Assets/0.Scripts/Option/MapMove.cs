using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    [SerializeField] Transform[] map;

    Vector3 pos = Vector3.zero;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            MapMoves(collision.transform.position);
        }
    }

    private void Update()
    {
        if (GameManager.instance.player == null) return;

        Vector3 pos = GameManager.instance.player.transform.position;
        if(Vector3.Distance(pos,this.pos) > 15)
        {
            MapMoves(pos);
        }
    }

    void MapMoves(Vector3 playerPos)
    {
        float up = Vector3.Distance(playerPos, pos + new Vector3(0, 20, 0));
        float down = Vector3.Distance(playerPos, pos + new Vector3(0, -20, 0));
        float left = Vector3.Distance(playerPos, pos + new Vector3(-20, 0, 0));
        float right = Vector3.Distance(playerPos, pos + new Vector3(20, 0, 0));


        float ul = Vector3.Distance(playerPos, pos + new Vector3(-20, 20, 0));
        float ur = Vector3.Distance(playerPos, pos + new Vector3(20, 20, 0));
        float dl = Vector3.Distance(playerPos, pos + new Vector3(-20, -20, 0));
        float dr = Vector3.Distance(playerPos, pos + new Vector3(20, -20, 0));

        switch (SmallLoc(up, down, left, right, ul, ur, dl, dr))
        {
            case 0:
                Up();
                break;
            case 1:
                Down();
                break;
            case 2:
                Left();
                break;
            case 3:
                Right();
                break;
            case 4:
                Up();
                Left();
                break;
            case 5:
                Up();
                Right();
                break;
            case 6:
                Down();
                Left();
                break;
            case 7:
                Down();
                Right();
                break;
        }
        transform.position = pos;
    }

    void Up()
    {
        Vector3 vtr = new Vector3(0, 40, 0);
        Rep(2, 0, vtr);
        Rep(3, 1, vtr);
        pos += vtr * 0.5f;
    }

    void Down()
    {
        Vector3 vtr = new Vector3(0, -40, 0);
        Rep(0, 2, vtr);
        Rep(1, 3, vtr);
        pos += vtr * 0.5f;
    }

    void Left()
    {
        Vector3 vtr = new Vector3(-40, 0, 0);
        Rep(1, 0, vtr);
        Rep(3, 2, vtr);
        pos += vtr * 0.5f;
    }
    void Right()
    {
        Vector3 vtr = new Vector3(40, 0, 0);
        Rep(0, 1, vtr);
        Rep(2, 3, vtr);
        pos += vtr * 0.5f;
    }

    int SmallLoc(params float[] numbers)
    {
        float small_number = 9999999;
        int small_loc = 0;

        for(int i=0; i< numbers.Length; i++)
        {
            if (numbers[i] < small_number)
            {
                small_loc = i;
                small_number = numbers[i];
            }
        }
        return small_loc;
    }

    public void Rep(int i, int j, Vector3 add)
    {
        Transform a = map[i];
        map[i] = map[j];
        map[j] = a;
        map[j].transform.position += add;
    }
}
