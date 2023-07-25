using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] private Transform target;  //target = player

    // Update is called once per frame
    void Update()
    {
        if(target != null)     //target가 없지 않을 때
        {
            Vector3 v1 = target.position;
            v1.z = -10f;
            transform.position = Vector3.Lerp(transform.position, v1, Time.deltaTime * 10f);
        }
    }
}
