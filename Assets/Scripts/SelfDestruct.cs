using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 3f;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, timeToDestroy); 
    }
}
