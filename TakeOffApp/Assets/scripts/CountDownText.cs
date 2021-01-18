using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Text))]
public class CountDownText : MonoBehaviour {

    public delegate void CountdownFinished();
    public static event CountdownFinished OnCountDownFinished;

    Text countdown;

    private void OnEnable() {
        countdown = GetComponent<Text>();
        countdown.text = "3";
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown() {
        GameObject.Find("Main Camera").GetComponent<GameManager>().gameOver = false;
        int count = 3;
        for (int i = 0; i < count; i++) {
            countdown.text = (count - i).ToString();
            yield return new WaitForSeconds(1);
        }
        OnCountDownFinished();
    }
}

