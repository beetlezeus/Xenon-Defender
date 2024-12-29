using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void RestartLevelWithDelay()
    {
        Invoke("RestartLevel", 1);
    }

    public void LoadNextStage()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex +1);
    }

    public void LoadFirstStage()
    {
        PersistentGameManager.Instance.ResetDeathState();
        PersistentGameManager.Instance.ResetLevelClearFlag();
        SceneManager.LoadScene(1);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void ReturnToMainWithDelay()
    {
        Invoke("ReturnToMain", 1);
        PersistentGameManager.Instance.transitionCanvas.SetActive(false);
    }
}
