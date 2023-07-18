using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isPickup = false;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickup)
        {
            transform.position = Vector3.Lerp(target.position, transform.position, 10f);
        }
    }
}
