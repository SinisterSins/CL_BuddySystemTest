using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{
    public TMP_Text scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PinManager.score = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + PinManager.score;
    }
}
