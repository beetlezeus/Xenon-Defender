using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLogic : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(this.name + " has collided with " + collision.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.name + " has collided with " + other.gameObject.name);
    }
}
