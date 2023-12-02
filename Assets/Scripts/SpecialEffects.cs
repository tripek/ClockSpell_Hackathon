using UnityEngine;

public class SpecialEffects : MonoBehaviour
{
    private BlockManager blockManager;

    public static SpecialEffects instance;
    [SerializeField] Sun sun;
    [SerializeField] float crocodileAscension = 0.2f;
    [SerializeField] float jaguarStopTime = 0.2f;
    [SerializeField] float monkeyMultiplier = 0.2f;
    [SerializeField] int eagleAdditionalPoints = 20;
    private ScoreManager scoreManager;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        blockManager = GetComponent<BlockManager>();

        scoreManager = GetComponent<ScoreManager>();
    }

    public void Crocodile()
    {
        sun.AscendTheSun(crocodileAscension);
    }

    public void Snake()
    {
        blockManager.Spawn7MultiBlock();
    }

    public void Monkey()
    {
        scoreManager.AddToScore((int) (scoreManager.GetScore() * monkeyMultiplier));
    }

    public void Eagle()
    {
        scoreManager.AddToScore(eagleAdditionalPoints);
    }

    public void Jaguar()
    {
        sun.StopTheSunForTime(jaguarStopTime);
    }
}
