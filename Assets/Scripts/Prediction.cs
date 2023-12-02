using System.Collections.Generic;
using UnityEngine;

public class Prediction : MonoBehaviour
{
    private Block[] blocks;
    private List<Block> blocksX0, blocksX1, blocksXmin1;

    private MultiBlock trackedMultiBlock;

    private float timer = 0.5f;

    private void Awake()
    {
        blocks = GetComponentsInChildren<Block>();

        UpdateBottomBlocks();
    }

    private void Update()
    {
        if (Time.timeScale <= 0 || !trackedMultiBlock) return;

        transform.position = new Vector3(trackedMultiBlock.transform.position.x, transform.position.y, 0);

        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            timer = 0.5f;
            UpdatePrediction();
        }
    }

    public void SetTrackedMultiBlock(MultiBlock multiBlock)
    {
        trackedMultiBlock = multiBlock;

        transform.localEulerAngles = Vector3.zero;

        blocks = GetComponentsInChildren<Block>();
        for (int i = 0; i < blocks.Length; i++)
        {
            GameObject childBlock = transform.GetChild(i).gameObject;

            if (trackedMultiBlock.transform.childCount > i)
            {
                Transform block = trackedMultiBlock.transform.GetChild(i);

                childBlock.transform.localPosition = block.localPosition;
                childBlock.SetActive(true);
            }
            else childBlock.SetActive(false);
        }
    }

    public void Rotate()
    {
        transform.localEulerAngles -= new Vector3(0f, 0f, 90f);

        UpdatePrediction();
    }

    public void UpdatePrediction()
    {
        if (!trackedMultiBlock) return;

        UpdateBottomBlocks();

        transform.position = trackedMultiBlock.transform.position;

        while (!BlocksBelow() && transform.position.y > Camera.main.transform.position.y - 5f)
        {
            transform.position -= new Vector3(0, 1, 0);
        }
    }

    private bool BlocksBelow()
    {
        foreach (Block block in blocks)
        {
            if (block.gameObject.activeSelf)
            {
                GameObject belowBlock = block.GetBelowBlock();

                if (belowBlock && belowBlock.transform.parent && belowBlock.transform.parent != trackedMultiBlock.transform)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private int Int(float x)
    {
        return Mathf.RoundToInt(x);
    }

    private void UpdateBottomBlocks()
    {
        int lowestY = 99;

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
            lowestY = 99;

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
}
