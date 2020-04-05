using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    public static int Score = 0;
    Text scoreText;

    void Start()
    {
        Score = 0;
        scoreText = GetComponent<Text>();
    }

    private void Update()
    {
        scoreText.text = Score.ToString();
    }
}
