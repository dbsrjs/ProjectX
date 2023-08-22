using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 v1 = target.position;
            v1.z = -10f;
            transform.position = Vector3.Lerp(transform.position, v1 , Time.deltaTime * 5f);
        } else
        {
            if(GameManager.instance.player != null)
            {
                target = GameManager.instance.player.transform;
            }
        }
    }
}
