using UnityEngine;

public class Block : MonoBehaviour
{
    public enum Symbol
    {
        none,
        crocodile,
        snake,
        monkey,
        eagle,
        jaguar
    }

    private BlockManager blockManager;

    private MultiBlock multiBlock;
    private bool bottomBlock, symbolActivated = true;

    [SerializeField] private Symbol symbol = Symbol.none;
    [SerializeField] private bool setSymbol = true;

    [SerializeField] private SpriteRenderer blockRenderer, symbolRenderer;
    [SerializeField] private ParticleSystem effectParticles;

    private void Awake()
    {
        if (!blockRenderer) return;

        blockManager = GameController.instance.GetComponent<BlockManager>();

        multiBlock = GetComponentInParent<MultiBlock>();

        blockRenderer.sprite = blockManager.GetRandomBlockSprite();

        if (setSymbol && Random.Range(0, 5) != 0)
        {
            symbol = (Symbol)Random.Range(1, 6);
            symbolRenderer.sprite = blockManager.GetSymbolByIndex((int)symbol - 1);

            effectParticles.startColor = blockManager.GetSymbolParticles((int)symbol - 1).startColor;
        }
    }

    private void Update()
    {
        if (Time.timeScale <= 0 || !multiBlock) return;

        if (bottomBlock && multiBlock.IsFalling())
        {
            GameObject below = GetBelowBlock();

            // if detects a block below, land the multiblock:
            if (below != null)
            {
                multiBlock.LandBlock();
            }
        }
    }

    public GameObject GetBelowBlock()
    {
        RaycastHit2D[] hitBlocks = Physics2D.RaycastAll(transform.position, Vector2.down, 0.6f);

        foreach (RaycastHit2D hitBlock in hitBlocks)
        {
            if (hitBlock && hitBlock.transform != transform && hitBlock.transform.parent != transform.parent)
            {
                // if detected a block below
                return hitBlock.transform.gameObject;
            }
        }

        return null;
    }

    public GameObject GetLeftBlock()
    {
        RaycastHit2D[] hitBlocks = Physics2D.RaycastAll(transform.position, Vector2.left, 0.6f);

        foreach (RaycastHit2D hitBlock in hitBlocks)
        {
            if (hitBlock && hitBlock.transform != transform)
            {
                // if detected a block left
                return hitBlock.transform.gameObject;
            }
        }

        return null;
    }

    public GameObject GetRightBlock()
    {
        RaycastHit2D[] hitBlocks = Physics2D.RaycastAll(transform.position, Vector2.right, 0.6f);

        foreach (RaycastHit2D hitBlock in hitBlocks)
        {
            if (hitBlock && hitBlock.transform != transform)
            {
                // if detected a block right
                return hitBlock.transform.gameObject;
            }
        }

        return null;
    }

    public void SetIfBottomBlock(bool bottom)
    {
        bottomBlock = bottom;
    }

    public bool IsBottomBlock()
    {
        return bottomBlock;
    }

    public Symbol GetSymbol()
    {
        return symbol;
    }

    public bool IsSymbolActivated()
    {
        return symbolActivated;
    }

    public void DeactivateSymbol()
    {
        symbolActivated = false;
        symbolRenderer.sprite = null;
    }

    public void EffectInvoked()
    {
        effectParticles.Play();

        symbolRenderer.sprite = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + 0.6f, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - 0.6f, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - 0.6f));

    }
}
