using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextLevel()
    {
        gameObject.GetComponent<GameController>().LoadNewSceneReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevelBefore()
    {
        gameObject.GetComponent<GameController>().LoadNewSceneReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ReloadLevel()
    {
        gameObject.GetComponent<GameController>().LoadNewSceneReset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
