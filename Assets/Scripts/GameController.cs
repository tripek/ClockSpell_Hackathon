using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject PauseScreen;
    public static GameController instance;
    private bool pausedGame;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        pausedGame = false;
    }

    private void Update()
    {
        if (PauseScreen != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pausedGame)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausedGame = true;
        PauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausedGame = false;
        PauseScreen.SetActive(false);
    }

    public void LoadNewSceneReset()
    {
        pausedGame = false;
        Time.timeScale = 1;
    }
}
