using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public Transform target;    //Monster
    private Monster monster;
    private float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (time >= 1f)
        {
            foreach (Transform child in transform)    //¾ÈµÊ
            {
                Debug.Log("test");
                Destroy(child.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
