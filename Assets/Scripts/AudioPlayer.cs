using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    void Awake()
    {
        int numAudioPlayers =  FindObjectsOfType<AudioPlayer>().Length;

        if(numAudioPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
