using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGrid : MonoBehaviour {
    [SerializeField] GameObject tilePrefab;
    int gridSize;
    GameObject[,] tiles;

    void Start() {
        gridSize = 4;
        tiles = new GameObject[gridSize, gridSize];
        CreateTiles();
    }

    private void CreateTiles() {
        for (int i = 0; i < gridSize - 1; i++) {
            for (int j = 0; j < gridSize; j++) {
                CreateTile(i, j);
            }
        }
        for (int j = 0; j < gridSize - 1; j++) {
            CreateTile(gridSize - 1, j);
        }
    }

    private void CreateTile(int i, int j) {
        tiles[i, j] = Instantiate(tilePrefab, new Vector3(j, -i, 0), Quaternion.identity);
        tiles[i, j].GetComponentInChildren<ClickableTile>().Init(this, i * gridSize + j);
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
        if (IsTileEmpty(r + 1, c)) {
            MoveTile(r, c, r + 1, c);
            return;
        }
        if (IsTileEmpty(r - 1, c)) {
            MoveTile(r, c, r - 1, c);
            return;
        }
        if (IsTileEmpty(r, c + 1)) {
            MoveTile(r, c, r, c + 1);
            return;
        }
        if (IsTileEmpty(r, c - 1)) {
            MoveTile(r, c, r, c - 1);
            return;
        }
    }

    private void MoveTile(int r1, int c1, int r2, int c2) {
        tiles[r1, c1].transform.position = new Vector3(c2, -r2, 0);
        tiles[r2, c2] = tiles[r1, c1];
        tiles[r1, c1] = null;
    }

    private bool IsTileEmpty(int r, int c) {
        if (r >= gridSize || r < 0 || c >= gridSize || c < 0) {
            return false;
        }
        return tiles[r, c] == null;
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
