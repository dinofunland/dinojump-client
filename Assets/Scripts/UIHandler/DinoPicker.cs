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

    // Start is called before the first frame update
    void Start()
    {

        var root = GetComponent<UIDocument>().rootVisualElement;
        dinoPicker = root.Q<VisualElement>("dinopicker");

        btnBlue = root.Q<Button>("blue");
        btnBlue.clicked += OnBlueDinoClicked;
        btnYellow = root.Q<Button>("yellow");
        btnYellow.clicked += OnYellowDinoClicked;
        btnGreen = root.Q<Button>("green");
        btnGreen.clicked += OnGreenDinoClicked;
        //int i = 0;
        //foreach (var dino in dinoSprites)
        //{
        //    Button dinoButton = new Button();
        //    dinoButton.style.backgroundImage = new StyleBackground{ };
        //    dinoButton.AddToClassList("dino-option");
        //    dinoButton.name = "Dino"+i.ToString();
        //    dinoButton.clicked += (delegate { OnDinoClicked(i); });
        //    dinoButton.visible = true;
        //    dinoButton.SetEnabled(true);

        //    switch (i)
        //    {
        //        default:
        //            dinoButton.AddToClassList("blueDinoSprite");
        //            break;
        //        case (int)DinoSkin.Green:
        //            dinoButton.AddToClassList("greenDinoSprite");
        //            break;
        //        case (int)DinoSkin.Yellow:
        //            dinoButton.AddToClassList("yellowDinoSprite");
        //            break;
        //    }
        //    dinoPicker.Add(dinoButton);
        //    i++;
        //}
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
    // Update is called once per frame
    void Update()
    {
        
    }

    public enum DinoSkin 
    {
        Blue = 0,
        Green = 1,
        Yellow = 2
    }
}
