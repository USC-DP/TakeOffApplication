using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGradient : MonoBehaviour {

    public int stageLength;
    public Color[] colors;

    private float t;
    private int colorIndex;

    private Renderer spriteRenderer;

    private bool privGameOver;
    private bool gameOver;

    private void OnEnable() {
        privGameOver = true;
        GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable() {
        GameManager.OnGameStarted -= OnGameStarted;
    }

    public void OnGameStarted() {
        privGameOver = false;
        t = 0.0f;
    }

    private void Start() {
        spriteRenderer = GetComponent<Renderer>();
        colorIndex = 0;
    }

    public int GetStageLength() {
        return stageLength;
    }
         
     void Update() {
        gameOver = GameObject.Find("Main Camera").GetComponent<GameManager>().GameOver();
        if (!gameOver && !privGameOver) {
            spriteRenderer.material.color = Color.Lerp(colors[colorIndex], colors[colorIndex + 1], t);
            if (t < 1) {
                t += Time.deltaTime / stageLength;
            }
            else if (colorIndex < colors.Length - 2) {
                colorIndex++;
                t = 0.0f;
            }
            else if (colorIndex == colors.Length - 1) {
                spriteRenderer.material.color = colors[colors.Length - 1];
            }
         }

    }

    public void setBack() {
        spriteRenderer.material.color = colors[0];
        Start();
        OnGameStarted();

    }
}
