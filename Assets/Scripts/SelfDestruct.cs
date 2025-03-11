using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  This is a dedicated script for ensuring instantiated particle systems self-desctruct after a a few seconds
 */

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 3f;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, timeToDestroy); 
    }
}
