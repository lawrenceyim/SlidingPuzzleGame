using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGrid : MonoBehaviour {
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject squarePrefab;
    [SerializeField] Sprite[] sprites;
    int gridSize;
    GameObject[,] tiles;
    int[,] MOVE_DIRECTION;
    float xOffset = -5f;
    float yOffset = 0f;

    void Start() {
        gridSize = 4;
        tiles = new GameObject[gridSize, gridSize];
        CreateTiles();
        MOVE_DIRECTION = new int[,] {
            {0, -1},
            {0, 1},
            {-1, 0},
            {1, 0}
        };
        ShufflePuzzle();
    }

    // Bottom right tile should always be empty in a correct solution
    private void CreateTiles() {
        for (int i = 0; i < gridSize - 1; i++) {
            for (int j = 0; j < gridSize; j++) {
                CreateTile(i, j);
                CreateSquare(i, j);
            }
        }
        for (int j = 0; j < gridSize - 1; j++) {
            CreateTile(gridSize - 1, j);
            CreateSquare(gridSize - 1, j);
        }
    }

    private void CreateTile(int i, int j) {
        tiles[i, j] = Instantiate(tilePrefab, new Vector3(j + xOffset, -i + yOffset, 0), Quaternion.identity);
        tiles[i, j].GetComponentInChildren<ClickableTile>().Init(this, i * gridSize + j, false);
        tiles[i, j].GetComponentInChildren<SpriteRenderer>().sprite = sprites[i * gridSize + j];
    }

    private void CreateSquare(int i, int j) {
        GameObject gameObject = Instantiate(squarePrefab, new Vector3(j, -i, 0), Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[i * gridSize + j];
    }

    public void MoveTile(GameObject tile) {
        int r = -1;
        int c = -1;
        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < gridSize; j++) {
                if (tiles[i, j] == tile) {
                    r = i;
                    c = j;
                    break;
                }
            }
        }
        for (int i = 0; i < MOVE_DIRECTION.GetLength(0); i++) {
            if (IsTileEmpty(r + MOVE_DIRECTION[i, 0], c + MOVE_DIRECTION[i, 1])) {
                MoveTile(r, c, r + MOVE_DIRECTION[i, 0], c + MOVE_DIRECTION[i, 1]);
                break;
            }
        }
        if (IsPuzzleComplete()) {
            Debug.Log("PUZZLE COMPLETED");
        }
    }

    private void ShufflePuzzle() {
        System.Random random = new System.Random();
        int shuffleTimes = 20;
        int emptyR = gridSize - 1;
        int emptyC = gridSize - 1;
        for (int i = 0; i < shuffleTimes; i++) {
            int randomNumber = random.Next(0, 4);
            int r = emptyR + MOVE_DIRECTION[randomNumber, 0];
            int c = emptyC + MOVE_DIRECTION[randomNumber, 1];
            if (IsTileOccupied(r, c)) {
                MoveTileWithoutVisualChange(r, c, emptyR, emptyC);
                emptyR = r;
                emptyC = c;
                continue;
            }
            i--;
        }
        UpdateTileVisualPositions();
    }

    private void MoveTile(int r1, int c1, int r2, int c2) {
        tiles[r1, c1].transform.position = new Vector3(c2 + xOffset, -r2 + yOffset, 0);
        tiles[r2, c2] = tiles[r1, c1];
        tiles[r1, c1] = null;
    }

    private void MoveTileWithoutVisualChange(int r1, int c1, int r2, int c2) {
        tiles[r2, c2] = tiles[r1, c1];
        tiles[r1, c1] = null;
    }

    private void UpdateTileVisualPositions() {
        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < gridSize; j++) {
                if (tiles[i, j] != null) {
                    tiles[i, j].transform.position = new Vector3(j + xOffset, -i + yOffset, 0);
                }
            }
        }
    }

    private bool IsTileEmpty(int r, int c) {
        if (r >= gridSize || r < 0 || c >= gridSize || c < 0) {
            return false;
        }
        return tiles[r, c] == null;
    }

    private bool IsTileOccupied(int r, int c) {
        if (r >= gridSize || r < 0 || c >= gridSize || c < 0) {
            return false;
        }
        return tiles[r, c] != null;
    }

    private bool IsPuzzleComplete() {
        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < gridSize; j++) {
                if (tiles[i, j] != null && !IsTileInCorrectSpot(i, j)) {
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsTileInCorrectSpot(int r, int c) {
        return tiles[r, c].GetComponentInChildren<ClickableTile>().GetTileNumber() == (r * gridSize + c);
    }

    private void PrintArray() {
        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < gridSize; j++) {
                string value = i.ToString() + " " + j.ToString();
                if (tiles[i, j] != null) {
                    value += " " + tiles[i, j].transform.position;
                }
                Debug.Log(value);

            }
        }
    }
}
