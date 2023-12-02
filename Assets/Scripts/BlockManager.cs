using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class BlockManager : MonoBehaviour
{
    private ScoreManager scoreManager;

    [SerializeField] private float placeCooldown = 1f, moveYSpeed = 1f, moveXCooldown = 1f;

    [SerializeField] private Vector2 spawnPos;
    [SerializeField] private Transform blockPositionsParent;
    [SerializeField] private Prediction prediction;
    [SerializeField] private Sun sun;

    [SerializeField] private MultiBlock multiBlock7Prefab;
    [SerializeField] private MultiBlock[] multiBlockPrefabs;

    [SerializeField] private Sprite[] blockSprites, symbolSprites;
    [SerializeField] private ParticleSystem[] symbolParticles;

    private int maxY = 0;
    private float placeTimer;
    private bool started;
    
    private MultiBlock currentMultiBlock;
    private MultiBlock[] nextMultiBlocks = new MultiBlock[3];

    private void Start()
    {
        scoreManager = GetComponent<ScoreManager>();

        SetCurrentMultiBlock(SpawnMultiBlock(spawnPos));
        currentMultiBlock.Activate();
    }

    private void Update()
    {
        if (Time.timeScale <= 0) return;

        if (currentMultiBlock)
        {
            if (Input.GetKeyDown(KeyCode.Space) && currentMultiBlock.CanFall() && placeTimer <= 0f)
            {
                currentMultiBlock.StartFalling();

                placeTimer = placeCooldown;

                if (!started)
                {
                    sun.ChangeTheSunSpeedMultiplier(1f);
                    started = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                currentMultiBlock.Rotate();
            }
        }
        else if (nextMultiBlocks.Length > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                ChooseNextBlock(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                ChooseNextBlock(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                ChooseNextBlock(2);
            }
        }

        if (placeTimer > 0f)
            placeTimer -= Time.deltaTime;
        else if (placeTimer != 0f)   
            placeTimer = 0f;
    }

    public void SetCurrentMultiBlock(MultiBlock multiBlock)
    {
        currentMultiBlock = multiBlock;
        if (!currentMultiBlock) return;

        currentMultiBlock.SetSpeed(moveYSpeed, moveXCooldown);
        currentMultiBlock.SetPrediction(prediction);

        prediction.SetTrackedMultiBlock(currentMultiBlock);
        prediction.gameObject.SetActive(true);
    }

    public void BlockLanded(MultiBlock multiBlock)
    {
        currentMultiBlock.SetPrediction(null);

        currentMultiBlock = null;
        prediction.gameObject.SetActive(false);

        ShowNextMultiBlocks();

        if (multiBlock.GetHighestBlock().transform.position.y > maxY)
        {
            maxY = Mathf.RoundToInt(multiBlock.GetHighestBlock().transform.position.y);

            spawnPos.y = maxY + 4;
            
            //Camera.main.transform.position = new Vector3(0, maxY, -10);
            Camera.main.transform.DOMoveY(maxY, 0.2f);
        }

        foreach (Block block in multiBlock.GetComponentsInChildren<Block>())
        {
            if (Mathf.RoundToInt(block.transform.position.y) == maxY)
                scoreManager.AddToScore(1);
            else
                scoreManager.AddToScore(4);
        }
    }

    public void BlockFellOutOfView()
    {
        currentMultiBlock = null;
        prediction.gameObject.SetActive(false);

        ShowNextMultiBlocks();

        scoreManager.AddToScore(-15);
    }

    private void ShowNextMultiBlocks()
    {
        blockPositionsParent.gameObject.SetActive(true);

        for (int i = 0; i < nextMultiBlocks.Length; i++)
        {
            nextMultiBlocks[i] = SpawnMultiBlock(blockPositionsParent.GetChild(i).position);
        }
    }

    private MultiBlock SpawnMultiBlock(Vector2 pos)
    {
        MultiBlock prefab = multiBlockPrefabs[Random.Range(0, multiBlockPrefabs.Length)];

        MultiBlock newMultiBlock = Instantiate(prefab, pos, Quaternion.identity);
        newMultiBlock.gameObject.name = prefab.name;

        return newMultiBlock;
    }

    private void ChooseNextBlock(int index)
    {
        blockPositionsParent.gameObject.SetActive(false);

        for (int i = 0; i < nextMultiBlocks.Length; i++)
        {
            if (i == index)
            {
                SetCurrentMultiBlock(nextMultiBlocks[i]);

                currentMultiBlock.transform.DOMove(spawnPos, 0.2f);

                currentMultiBlock.Activate();
            }
            else nextMultiBlocks[i].Destroy();
        }

        nextMultiBlocks = new MultiBlock[3];

        AudioPlayer.instance.PlayAudio("take");
    }

    public void Spawn7MultiBlock()
    {
        MultiBlock newMultiBlock = Instantiate(multiBlock7Prefab, new Vector3(0, maxY + 1), Quaternion.identity);
        newMultiBlock.gameObject.name = multiBlock7Prefab.name;
    }

    public Sprite GetRandomBlockSprite()
    {
        return blockSprites[Random.Range(0, blockSprites.Length)];
    }

    public Sprite GetSymbolByIndex(int index)
    {
        return symbolSprites[index];
    }

    public ParticleSystem GetSymbolParticles(int index)
    {
        return symbolParticles[index];
    }
}
