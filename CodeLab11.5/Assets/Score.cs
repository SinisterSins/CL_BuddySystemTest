using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;

    void Start()
    {
        Pin.score = 0;
    }

    void Update()
    {
        scoreText.text = "Score: " + Pin.score;
    }
}