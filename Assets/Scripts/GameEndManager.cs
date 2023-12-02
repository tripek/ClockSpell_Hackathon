using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    private ScoreManager scoreManager;

    [SerializeField] private Transform sunTransform;
    [SerializeField] private GameObject gameEndedScreen;
    [SerializeField] private TextMeshProUGUI scoreText;
    private bool gameEnded;

    private void Start()
    {
        scoreManager = GetComponent<ScoreManager>();

        gameEnded = false;
    }

    private void Update()
    {
        if (sunTransform.localPosition.y <= -10 && !gameEnded)
        {
            GameEnded();
        }
        if(gameEnded && Input.GetKeyDown(KeyCode.R)) 
        {
            GameController.instance.GetComponent<SceneLoader>().ReloadLevel();
        }
    }

    private void GameEnded()
    {
        Time.timeScale = 0;
        gameEndedScreen.SetActive(true);
        scoreText.text = "Score: " + scoreManager.GetScore().ToString();
        gameEnded = true;
    }
}
