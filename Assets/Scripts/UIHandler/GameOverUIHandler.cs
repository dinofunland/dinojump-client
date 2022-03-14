using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUIHandler : MonoBehaviour
{
    Label scoreText;
    Label rankText;

    void OnEnable()
    {
        InitializeUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeUI()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        scoreText = root.Q<Label>("score-text");

        scoreText.text = "Score: " + GameManager.Instance.Score;
    }
}
