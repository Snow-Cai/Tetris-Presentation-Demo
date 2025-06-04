using UnityEngine;
using System.Collections.Generic;

public class SpawnTetris : MonoBehaviour
{
    public GameObject[] TetrisBlocks;

    private GameObject nextBlock;

    void Start()
    {
        nextBlock = GetSmartBlock();
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        Instantiate(nextBlock, transform.position, Quaternion.identity);
        nextBlock = GetSmartBlock();
    }

    GameObject GetSmartBlock()
    {
        bool needsIBlock = CheckForVerticalSlot(heightNeeded: 4);

        if (needsIBlock)
        {
            List<GameObject> weightedPool = new List<GameObject>(TetrisBlocks);

            foreach (GameObject block in TetrisBlocks)
            {
                if (block.name.ToLower().Contains("i"))
                {
                    weightedPool.Add(block); // Increase I block weight
                    break;
                }
            }

            return weightedPool[Random.Range(0, weightedPool.Count)];
        }

        // Default random selection
        return TetrisBlocks[Random.Range(0, TetrisBlocks.Length)];
    }

    bool CheckForVerticalSlot(int heightNeeded)
    {
        Transform[,] grid = TetrisMove.Grid;

        for (int x = 0; x < TetrisMove.width; x++)
        {
            int freeCount = 0;

            for (int y = 0; y < TetrisMove.height; y++)
            {
                if (grid[x, y] == null)
                {
                    freeCount++;

                    if (freeCount >= heightNeeded)
                        return true; // Found a vertical slot
                }
                else
                {
                    freeCount = 0; // Reset if blocked
                }
            }
        }

        return false;
    }
}
