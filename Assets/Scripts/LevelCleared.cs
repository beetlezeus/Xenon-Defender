using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCleared : MonoBehaviour
{

    public void SetLevelClearedFlag()
    {
        PersistentGameManager.Instance.levelCleared = true;
        Invoke("ShowTransitionScreen", 1f);
    }

    public void ShowTransitionScreen()
    {
        PersistentGameManager.Instance.ShowTransitionScreen(false);
    }
}
