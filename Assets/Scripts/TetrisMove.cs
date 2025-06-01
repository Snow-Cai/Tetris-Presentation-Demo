using UnityEngine;
using System;

public class TetrisMove : MonoBehaviour
{
    private float previousTime;
    public float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    public Vector3 rotationPoint;

    private static Transform[,] grid = new Transform[width, height]; // stores 2D array of transforms

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.isPaused) return;

        Move();

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);            
            if (!ValidMove())
            {
                transform.position += new Vector3(0, 1, 0);
                AddToGrid(); // adds the block to the grid when it touches the ground
                AudioManager.Instance.PlaySFX(AudioManager.Instance.blockPlacedSound);
                CheckClear();

                this.enabled = false; // stop the script when the block touches the ground
                if (!GameManager.Instance.isGameOver)
                    FindObjectOfType<SpawnTetris>().SpawnBlock(); // spawn a new block
            }
            previousTime = Time.time;
        }
    }

    void Move()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position += new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position += new Vector3(-1, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
                AudioManager.Instance.PlayRotateSound(false); // play error sound
            }
            else
            {
                AudioManager.Instance.PlayRotateSound(true); // play success sound
            }
        }
    }

    void AddToGrid() // track the block to the grid
    {
        foreach (Transform child in transform)
        {
            int posX = Mathf.RoundToInt(child.transform.position.x);
            int posY = Mathf.RoundToInt(child.transform.position.y);

            if (posY >= height - 2)
            {
                GameManager.Instance.GameOver();
                return;
            }

            if (posY < height)
                grid[posX, posY] = child;
        }
    }

    void CheckClear() // manage the line clear
    {
        int linesCleared = 0;

        for (int i = height - 1; i >= 0; i--)
        {
            if (IsLineFull(i))
            {
                ClearLine(i);
                MoveAllLinesDown(i);
                i++; // recheck the line after moving down
                linesCleared++;
            }
        }

        if (linesCleared > 0)
        {
            GameManager.Instance.AddScore(linesCleared);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.lineClearSound);
        }
    }

    bool IsLineFull(int y) // check if the line is full
    {
        for (int i = 0; i < width; i++)
        {
            if (grid[i, y] == null)
                return false;
        }
        return true;
    }

    void ClearLine(int y) // clear the line
    {
        for (int i = 0; i < width; i++)
        {
            Destroy(grid[i, y].gameObject); // destroy the block from grid
            grid[i, y] = null;
        }
    }

    void MoveAllLinesDown(int y) // move all leftover block parts down
    {
        for (int i = y + 1; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, i] != null)
                {
                    grid[j, i - 1] = grid[j, i];
                    grid[j, i] = null;
                    grid[j, i - 1].transform.position += Vector3.down;
                }
            }
        }
    }

    bool ValidMove() // sfx for err
    {
        foreach (Transform child in transform)
        {
            int posX = Mathf.RoundToInt(child.transform.position.x);
            int posY = Mathf.RoundToInt(child.transform.position.y);

            if (posX < 0 || posX >= width || posY < 0 || posY >= height)
            {
                return false;
            }

            if (grid[posX, posY] != null)
            {
                return false;
            }
        }
        return true;
    }
}