using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickableTile : MonoBehaviour {
    PuzzleGrid puzzleGrid;
    int tileNumber;
    TextMeshPro number;

    public void Init(PuzzleGrid puzzleGrid, int tileNumber) {
        this.puzzleGrid = puzzleGrid;
        this.tileNumber = tileNumber;
        this.number = transform.parent.gameObject.GetComponentInChildren<TextMeshPro>();
        this.number.text = tileNumber.ToString();
    }

    private void OnMouseDown() {
        puzzleGrid.MoveTile(this.transform.parent.gameObject);
    }
}
