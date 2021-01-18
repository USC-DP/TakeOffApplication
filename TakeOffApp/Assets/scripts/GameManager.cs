using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    //public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;
    public Text highScore;
    public AudioSource soundTrack;

    private Vector3 playerPos;

    int score = 0;
    public bool gameOver = true;

    enum PageState {
        None,
        Start,
        GameOver,
        CountDown
    }

    private void Awake() {
        Instance = this;
        SetPageState(PageState.Start);
        playerPos = GameObject.Find("Player").GetComponent<Transform>().position;
    }

    private void OnEnable() {
        CountDownText.OnCountDownFinished += OnCountDownFinished;
        PlayerControl.OnPlayerDied += OnPlayerDied;

    }

    private void OnDisable() {
        CountDownText.OnCountDownFinished -= OnCountDownFinished;
        PlayerControl.OnPlayerDied -= OnPlayerDied;
    }

    public bool GameOver() {
        return gameOver;
    }

    private void OnPlayerDied() {
        gameOver = true;
        CancelInvoke("IncreaseScore");
        highScore.text = "You Lasted " + score.ToString() + " Seconds";
        SetPageState(PageState.GameOver);
    }

    void OnCountDownFinished() {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
        InvokeRepeating("IncreaseScore", 0.0f, 1.0f);
    }

    void IncreaseScore() {
        score++;
        if (!gameOver) {
            scoreText.text = "T-Plus: " + score.ToString() + " Seconds";
        }
    }
    public int GetScore() {
        return score;
    }

    void SetPageState(PageState state) {
        switch (state) {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;

            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;

            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;

            case PageState.CountDown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }

    public void Retry() {
        soundTrack.Stop();
        SetPageState(PageState.Start);
        score = 0;
        GameObject.Find("Player").GetComponent<Transform>().position = playerPos;
        scoreText.text = "T-Plus: " + score.ToString() + " Seconds";
        GameObject.Find("ObstacleSpawner").GetComponent<Parallaxer>().OnDisable();
        GameObject.Find("Background").GetComponent<SpriteGradient>().setBack();
        GameObject.Find("Score Text").GetComponent<TextGradient>().OnGameStarted();
    }

    public void StartGame() {
        soundTrack.Play(0);
        SetPageState(PageState.CountDown);
        scoreText.text = "T-Plus: 0 Seconds";
        GameObject.Find("ObstacleSpawner").GetComponent<Parallaxer>().OnDisable();
        GameObject.Find("ObstacleSpawner").GetComponent<Parallaxer>().resetShift();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
