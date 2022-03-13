using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DinoPicker : MonoBehaviour
{
    public List<Sprite> dinoSprites;
    VisualElement dinoPicker;
    public DinoSkin ActiveDino = 0;

    Button btnBlue;
    Button btnYellow;
    Button btnGreen;
    Button btnPurple;

    // Start is called before the first frame update
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        dinoPicker = root.Q<VisualElement>("dinopicker");

        btnBlue = root.Q<Button>("blue");
        btnBlue.clicked += OnBlueDinoClicked;
        btnYellow = root.Q<Button>("yellow");
        btnYellow.clicked += OnYellowDinoClicked;
        btnGreen = root.Q<Button>("green");
        btnGreen.clicked += OnGreenDinoClicked;
        btnPurple = root.Q<Button>("purple");
        btnPurple.clicked += OnPurpleDinoClicked;
    }

    private void OnBlueDinoClicked()
    {
        Debug.Log("Blue Dino sent.");

        ActiveDino = DinoSkin.Blue;
        SendSkinInfo();
        ResetButtonActive();    
        btnBlue.AddToClassList("dino-option-active");
    }

    void ResetButtonActive()
    {
        foreach (var child in dinoPicker.Children())
        {
            child.RemoveFromClassList("dino-option-active");
        }
    }

    private void OnYellowDinoClicked()
    {
        Debug.Log("Yellow Dino sent.");
        ActiveDino = DinoSkin.Yellow;
        SendSkinInfo();
        ResetButtonActive();
        btnYellow.AddToClassList("dino-option-active");
    }
    private void OnGreenDinoClicked()
    {
        Debug.Log("Green Dino sent.");
        ActiveDino = DinoSkin.Green;
        SendSkinInfo();
        ResetButtonActive();
        btnGreen.AddToClassList("dino-option-active");
    }
    private void OnPurpleDinoClicked()
    {
        Debug.Log("Purple Dino sent.");
        ActiveDino = DinoSkin.Purple;
        SendSkinInfo();
        ResetButtonActive();
        btnPurple.AddToClassList("dino-option-active");
    }

    private async void SendSkinInfo()
    {
        if (RoomManager.Instance?.colyseusRoom != null)
        {
            await RoomManager.Instance.colyseusRoom.Send("selectSkin", new { skin = ActiveDino });
        }
        else 
        {
            Debug.LogError("No connection to Server.");
        }
    }

    public enum DinoSkin 
    {
        Blue = 0,
        Green = 1,
        Yellow = 2,
        Purple = 3
    }
}
