using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public Transform target;
    private Monster monster;
    // Update is called once per frame
    void Update()
    {
        
    }
    

    IEnumerator Bomb()
    {
        float distance = Vector3.Distance(transform.position, target.position);    //둘 사이의 거리 계산
        yield return new WaitForSeconds(3f);

        if (distance <= 3f)
        {
            monster.GetComponent<Monster>().Dead(0.5f, 500);
        }
        Destroy(transform);
    }
}
