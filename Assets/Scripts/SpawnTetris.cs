using UnityEngine;

public class SpawnTetris : MonoBehaviour
{
    public GameObject[] TetrisBlocks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        Instantiate(TetrisBlocks[Random.Range(0, TetrisBlocks.Length)], transform.position, Quaternion.identity);
    }
}
