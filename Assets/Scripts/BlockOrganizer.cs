using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BlockOrganizer : MonoBehaviour
{
    private int filledBlocks;
    
    struct BlockPos
    {
        public float y;
        public Block block;
    };

    public void OrganizeBlocks(Block[] blocks)
    {
        int clearedRows = 0;
        int symbolCombos = 0;

        List<BlockPos> blockHeights = new List<BlockPos>();

        foreach (Block block in blocks)
        {
            if (blockHeights.Count == 0 || !blockHeights.Any(element => element.y == block.transform.position.y))
                blockHeights.Add(new BlockPos() {y = block.transform.position.y, block = block});
        }

        foreach (BlockPos blockPos in blockHeights)
        {
            //Debug.Log("Main: " + blockPos.block.name);
            //Debug.Log("Pos: " + blockPos.block.transform.position);
            filledBlocks = 1;
            CheckForSideBlocks(blockPos.block);
            //Debug.Log("Blocks in row: " + filledBlocks);
            if (filledBlocks == 7)
                clearedRows++;
        }


    }

    private void CheckForSideBlocks(Block block)
    {
        CheckForLeftBlock(block);
        CheckForRightBlock(block);
    }

    private void CheckForLeftBlock(Block block)
    {
        GameObject leftBlock = block.GetLeftBlock();

        if (leftBlock != null)
        {
            //Debug.Log("Left block: " + leftBlock.name);
            filledBlocks++;
            CheckForLeftBlock(leftBlock.GetComponent<Block>());
        }
    }

    private void CheckForRightBlock(Block block)
    {
        GameObject rightBlock = block.GetRightBlock();

        if (rightBlock != null)
        {
            //Debug.Log("Right block: " + rightBlock.name);
            filledBlocks++;
            CheckForRightBlock(rightBlock.GetComponent<Block>());
        }
    }
}
