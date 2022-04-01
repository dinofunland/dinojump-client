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
        var root = GetComponent<UIDocument>().rootVisualElement;
        scoreText = root.Q<Label>("score-text");
        SetLavaMeterVisibility(root);
    }

    void SetLavaMeterVisibility(VisualElement root)
    {
        var lavaMeter = root.Q<VisualElement>("LavaMeterBG");
        if (PlayerPrefs.HasKey("hideMeter"))
        {
            if (PlayerPrefs.GetInt("hideMeter") > 0)
            {
                lavaMeter.style.display = DisplayStyle.None;
                gameObject.GetComponent<LavaMeter>().enabled = false;
            }
            else
            {
                lavaMeter.style.display = DisplayStyle.Flex;
                gameObject.GetComponent<LavaMeter>().enabled = true;
            }
        }
    }

    internal void SetNewScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
