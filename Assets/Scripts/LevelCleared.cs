using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LevelCleared : MonoBehaviour
{

    public void SetLevelClearedFlag()
    {
        PersistentGameManager.Instance.levelCleared = true;
        Invoke("ShowTransitionScreen", 1f);
    }

    public async Task ShowTransitionScreen()
    {
        await PersistentGameManager.Instance.ShowTransitionScreen(false);
    }
}
