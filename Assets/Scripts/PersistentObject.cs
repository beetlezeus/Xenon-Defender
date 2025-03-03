using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static PersistentObject instance;

    //Awake is called before Start(). used to enforce the Singleton pattern.
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameObject alive across scenes
            Debug.Log("PersistentManager created and marked as DontDestroyOnLoad.");
        }
        else
        {
            Debug.Log("Duplicate PersistentManager found, destroying it.");
            Destroy(gameObject); // Enforce single instance
        }
    }

    void OnDestroy()
    {
        Debug.Log("PersistentManager is being destroyed.");
    }
}