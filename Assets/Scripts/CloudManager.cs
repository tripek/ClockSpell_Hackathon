using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    private Transform cam;

    [SerializeField] private float spawnCooldown = 1f;

    [SerializeField] private Cloud cloudPrefab;
    [SerializeField] private Sprite[] cloudSprites;

    private List<Cloud> cloudPool = new List<Cloud>();

    private float spawnTimer;

    private void Start()
    {
        cam = Camera.main.transform;

        spawnTimer = spawnCooldown;
    }

    void Update()
    {
        if (Time.timeScale <= 0 || cam.position.y < 16f) return;

        if (spawnTimer > 0)
            spawnTimer -= Time.deltaTime;
        else
        {
            spawnTimer = spawnCooldown + Random.Range(-0.5f, 1f);

            SpawnCloud();
        }
    }

    private void SpawnCloud()
    {
        Vector2 spawnPos = new Vector2(13f, cam.transform.position.y + Random.Range(-3f, 3f));
        Vector2 dir = Vector2.left;

        if (Random.Range(0, 2) == 0)
        {
            spawnPos.x = -13f;
            dir = Vector2.right;
        }

        Cloud newCloud;

        if (cloudPool.Count == 0)
            newCloud = Instantiate(cloudPrefab, spawnPos, Quaternion.identity);
        else
        {
            newCloud = cloudPool[0];
            cloudPool.RemoveAt(0);

            newCloud.transform.position = spawnPos;

            newCloud.gameObject.SetActive(true);
        }

        newCloud.Setup(dir, cloudSprites[Random.Range(0, cloudSprites.Length)], this);
    }

    public void AddCloudToPool(Cloud cloud)
    {
        if (!cloudPool.Contains(cloud))
            cloudPool.Add(cloud);
    }

    public void RemoveCloudFromPool(Cloud cloud)
    {
        if (cloudPool.Contains(cloud))
            cloudPool.Remove(cloud);
    }
}
