using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Unity.Collections.AllocatorManager;
using UnityEngine.SceneManagement;

public class MultiBlock : MonoBehaviour
{
    private BlockManager blockManager;

    private BlockOrganizer blockOrganizer;

    private Block[] blocks;

    private List<Block> blocksX0, blocksX1, blocksXmin1;

    private bool active, isFalling, movingRight, inPosition;

    private float moveYSpeed, moveXCooldown, maxX = 3f, moveTimer, startY;

    private Transform cam;

    private Prediction prediction;

    private Transform leftBlock, rightBlock;

    private void Awake()
    {
        blockManager = GameController.instance.GetComponent<BlockManager>();
        blockOrganizer = GameController.instance.GetComponent<BlockOrganizer>();

        blocks = GetComponentsInChildren<Block>();

        cam = Camera.main.transform;

        startY = transform.position.y;

        UpdateBottomBlocks();
        UpdateLeftBlock();
        UpdateRightBlock();

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f);
    }

    private void Update()
    {
        if (Time.timeScale <= 0) return;

        if (active && !inPosition)
        {
            if (isFalling)
            {
                // fall
                transform.position -= new Vector3(0f, moveYSpeed * Time.deltaTime, 0f);

                if (transform.position.y < cam.position.y - 7f)
                {
                    blockManager.BlockFellOutOfView();

                    DestroyObject();
                }
            }
            else // is not falling
            {
                // move left and right

                if (!movingRight)
                { // moving left

                    if (leftBlock.position.x > -maxX)
                    {
                        if (moveTimer > 0f)
                            moveTimer -= Time.deltaTime;
                        else
                        {
                            moveTimer = moveXCooldown;
                            
                            //transform.position -= new Vector3(1, 0f, 0f);
                            transform.DOMoveX(transform.position.x - 1, 0.15f);

                            if (prediction)
                                prediction.UpdatePrediction();
                        }
                    }
                    else // reached max pos
                    {
                        movingRight = true;
                        RoundPosToInt();
                    }
                }
                else if (movingRight)
                { // moving right

                    if (rightBlock.position.x < maxX)
                    {
                        if (moveTimer > 0f)
                            moveTimer -= Time.deltaTime;
                        else
                        {
                            moveTimer = moveXCooldown;

                            //transform.position += new Vector3(1, 0f, 0f);
                            transform.DOMoveX(transform.position.x + 1, 0.15f);

                            if (prediction)
                                prediction.UpdatePrediction();
                        }
                    }
                    else // reached max pos
                    {
                        movingRight = false;
                        RoundPosToInt();
                    }
                }
            }
        }
    }

    private int Int(float x)
    {
        return Mathf.RoundToInt(x);
    }

    public void RoundPosToInt()
    {
        DOTween.CompleteAll();
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
    }
    
    public bool IsOnFullPos()
    {
        return transform.position == new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
    }

    public void LandBlock()
    {
        if (inPosition) return;

        isFalling = false;
        inPosition = true;

        RoundPosToInt();

        blockManager.BlockLanded(this);
        blockOrganizer.OrganizeBlocks(blocks);
        foreach(Block block in blocks)
        {
            block.GetComponent<SymbolsChecker>().StartChecking();
        }

        AudioPlayer.instance.PlayAudio("land");
    }

    public void Rotate()
    {
        if (isFalling || inPosition) return;

        transform.localEulerAngles -= new Vector3(0f, 0f, 90f);
        //if (DOTween.TotalActiveTweens() == 0)
        //    transform.DORotate(transform.localEulerAngles + new Vector3(0f, 0f, 90f), 0.15f);

        if (prediction)
            prediction.Rotate();

        UpdateBottomBlocks();
        UpdateLeftBlock();
        UpdateRightBlock();

        RoundPosToInt();
    }

    public void Activate()
    {
        active = true;
    }

    public void StartFalling()
    {
        RoundPosToInt();
        isFalling = true;
    }

    public void Destroy()
    {
        transform.DOScale(Vector3.zero, 0.3f);
        Invoke(nameof(DestroyObject), 0.3f);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void UpdateBottomBlocks()
    {
        int lowestY = 9999;

        blocksX0 = new List<Block>();
        blocksX1 = new List<Block>();
        blocksXmin1 = new List<Block>();

        foreach (Block block in blocks)
        {
            switch (Int(block.transform.position.x) - Int(transform.position.x))
            {
                case 0:
                    blocksX0.Add(block);
                    break;
                case 1:
                    blocksX1.Add(block);
                    break;
                case -1:
                    blocksXmin1.Add(block);
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            lowestY = 9999;

            Block[] theBlocks = new Block[6];
            if (i == 0) blocksX0.CopyTo(theBlocks);
            else if (i == 1) blocksX1.CopyTo(theBlocks);
            else if (i == 2) blocksXmin1.CopyTo(theBlocks);

            foreach (Block block in theBlocks)
            {
                if (!block) break;

                if (block.transform.position.y < lowestY)
                    lowestY = (int)block.transform.position.y;
            }

            foreach (Block block in theBlocks)
            {
                if (!block) break;

                if (block.IsBottomBlock())
                    block.SetIfBottomBlock(false);

                if ((int)block.transform.position.y == lowestY)
                { // set this block as bottom block

                    block.SetIfBottomBlock(true);
                }
            }
        }
    }

    private void UpdateLeftBlock()
    {
        int minX = 9999;

        foreach (Block block in blocks)
        {
            if (block.transform.position.x < minX)
                minX = (int)block.transform.position.x;
        }

        foreach (Block block in blocks)
        {
            if ((int)block.transform.position.x == minX)
            { // set this block as left block

                leftBlock = block.transform;
                break;
            }
        }
    }

    private void UpdateRightBlock()
    {
        int maxX = -9999;

        foreach (Block block in blocks)
        {
            if (block.transform.position.x > maxX)
                maxX = (int)block.transform.position.x;
        }

        foreach (Block block in blocks)
        {
            if ((int)block.transform.position.x == maxX)
            { // set this block as right block

                rightBlock = block.transform;
                break;
            }
        }
    }

    public Block GetHighestBlock()
    {
        float maxY = -9999;
        int index = 0;

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].transform.position.y > maxY)
            {
                maxY = blocks[i].transform.position.y;
                index = i;
            }
        }

        return blocks[index];
    }

    public void SetPrediction(Prediction pred)
    {
        prediction = pred;
    }

    public bool CanFall()
    {
        foreach (Block block in blocks)
        {
            if (block.IsBottomBlock() && block.GetBelowBlock())
                return false;
        }

        return true;
    }

    public void SetSpeed(float ySpeed, float xCooldown)
    {
        moveYSpeed = ySpeed;
        moveXCooldown = xCooldown;

        moveTimer = moveXCooldown;
    }

    public bool IsFalling() { return isFalling; }
}
