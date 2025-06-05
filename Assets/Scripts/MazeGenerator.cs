using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;

    [SerializeField] private Collectible collectiblePrefab;

    [SerializeField] private int _mazeWidth;

    [SerializeField] private int _mazeDepth;

    private MazeCell[,] _mazeGrid;

    private MazeCell _currentPlayerCell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        // Generate the initial maze starting at the center
        yield return GenerateMaze(null, _mazeGrid[_mazeWidth / 2, _mazeDepth / 2]);

        // Place the first collectible randomly in the maze
        PlaceCollectible();
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        yield return new WaitForSeconds(0.1f);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    public void RegenerateMaze(Vector3 playerPosition)
    {
        int playerX = Mathf.RoundToInt(playerPosition.x);
        int playerZ = Mathf.RoundToInt(playerPosition.z);

        // Reset maze state
        foreach (var cell in _mazeGrid)
        {
            cell.Reset();
        }

        // Start maze regeneration from the player's current position
        StartCoroutine(GenerateMaze(null, _mazeGrid[playerX, playerZ]));

        // Place a new collectible randomly
        PlaceCollectible();
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(0, _mazeWidth);
        float z = Random.Range(0, _mazeDepth);

        return new Vector3(x, 1f, z);
    }

    private Vector3 GetRandomRotation()
    {
        float x = Random.Range(0, 90);
        float y = Random.Range(0, 90);
        float z = Random.Range(0, 90);

        return new Vector3(x, y, z);
    }

    public void PlaceCollectible()
    {
        Vector3 position = GetRandomPosition();
        Quaternion rotation = Quaternion.Euler(GetRandomRotation()); // Convert Vector3 to Quaternion

        Instantiate(collectiblePrefab, position, rotation);
    }
}
