using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGradient : MonoBehaviour {

    public float stageLength;
    public Color[] colors;

    private float t;
    private int colorIndex;

    private Text text;

    private bool privGameOver;

    private void OnEnable() {
        privGameOver = true;
        GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable() {
        GameManager.OnGameStarted -= OnGameStarted;
    }

    public void OnGameStarted() {
        privGameOver = false;
        colorIndex = 0;
        t = 0.0f;
    }

    private void Start() {
        text = GetComponent<Text>();
    }

    void Update() {
        if (!privGameOver) {
            text.color = Color.Lerp(colors[colorIndex], colors[colorIndex + 1], t);
            if (t < 1) {
                t += Time.deltaTime / stageLength;
            }
            else if (colorIndex < colors.Length - 2) {
                colorIndex++;
                t = 0.0f;
            }
            else if (colorIndex == colors.Length - 1) {
                text.color = colors[colors.Length - 1];
            }
        }
    }

    public void setBack() {
        text.color = colors[0];
        Start();
        OnGameStarted();
    }
}
