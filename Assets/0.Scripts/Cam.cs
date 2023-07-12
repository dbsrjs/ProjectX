using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            //Vector3 vec = new Vector3(target.position.x, target.position.y, -10f);
        }
    }
}
