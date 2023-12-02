using UnityEngine;

public class Cloud : MonoBehaviour
{
    private CloudManager cloudManager;

    [SerializeField] private float moveSpeed = 1f;

    private SpriteRenderer sRenderer;

    private float targetX;
    private Vector3 moveDir;

    private void Update()
    {
        if (Time.timeScale <= 0) return;

        if (moveDir.x > 0)
        { // going right

            if (transform.position.x > targetX)
                OutOfView();
            else
                Move();
        }
        else if (moveDir.x < 0)
        { // going left

            if (transform.position.x < targetX)
                OutOfView();
            else
                Move();
        }
    }

    private void Move()
    {
        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }

    private void OutOfView()
    {
        cloudManager.AddCloudToPool(this);
        gameObject.SetActive(false);
    }

    public void Setup(Vector2 dir, Sprite sprite, CloudManager manager)
    {
        moveDir = dir;
        targetX = 13f;
        if (moveDir.x < 0)
            targetX = -13f;

        moveSpeed += Random.Range(-1f, 1f);

        if (!sRenderer)
            sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.sprite = sprite;

        sRenderer.sortingOrder = -90;
        if (Random.Range(0, 2) == 0) sRenderer.sortingOrder = 40;

        if (!cloudManager)
            cloudManager = manager;
    }
}
