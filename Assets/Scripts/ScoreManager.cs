using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    private int score;

    [SerializeField] private TextMeshProUGUI scoreText;

    private void SetScore(int s)
    {
        score = s;

        scoreText.text = score.ToString();

        if (DOTween.TotalTweensById(-34.034) == 0)
            scoreText.transform.DOPunchScale(new Vector2(0.2f, 0.2f), 0.15f).id = -34.034;
    }

    public void AddToScore(int s)
    {
        SetScore(score + s);
    }

    public int GetScore()
    {
        return score;
    }
}
