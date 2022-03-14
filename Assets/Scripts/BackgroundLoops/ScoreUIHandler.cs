using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreUIHandler : MonoBehaviour
{
    private Label scoreText;
    // Start is called before the first frame update
    void OnEnable()
    {
        scoreText = GetComponent<UIDocument>().rootVisualElement.Q<Label>("score-text");
    }

    internal void SetNewScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
}
