using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOverHighlightTile : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color originalColor = Color.white;
    Color highLightColor = new Color(255, 255, 0, 0.5f);

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMouseOver() {
        spriteRenderer.color = highLightColor;
    }

    public void OnMouseExit() {
        spriteRenderer.color = originalColor;
    }
}
