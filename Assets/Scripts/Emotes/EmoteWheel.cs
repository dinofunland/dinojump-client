using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteWheel : MonoBehaviour
{
    [SerializeField] Sprite[] emoteSprites;
    [SerializeField] GameObject[] wheelItems;

    List<Sprite> shownEmotes;
    public bool isWheelOpen;
    public bool emoteChosen;
    int currentIndex = 0;
    int currentSelectedEmote = 0;

    float lastVerticalInput = 0;
    bool lastEmoteButtonInput = true;
    float countDown = 0;
    bool isForward = true;


    public void OpenWheel()
    {
        countDown = 0;
        currentIndex = currentSelectedEmote-1;
        emoteChosen = false;
        isWheelOpen = true;
        foreach (var item in wheelItems)
        {
            item.GetComponent<SpriteRenderer>().enabled = true;
        }
        ShowWheel(currentIndex);
    }

    public void CloseWheel()
    {
        isWheelOpen = false;
        lastEmoteButtonInput = true;
        foreach (var item in wheelItems)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (!isWheelOpen)
        {
            return;
        }
        //Fire Emote
        var emoteButton = Input.GetButton("Emote");
        if (emoteButton != lastEmoteButtonInput)
        {
            lastEmoteButtonInput = emoteButton;
            if (emoteButton)
            {
                RoomManager.Instance?.colyseusRoom.Send("emote", currentSelectedEmote);
                CloseWheel();
                return;
            }
        }

        //Close After 3 Seconds of no Action
        if (countDown >= 3)
        {
            CloseWheel();
            return;
        }

        //Close Wheel
        if (Input.GetButton("Cancel"))
        {
            CloseWheel();
            return;
        }

        //Move WheelPosition

        var verticalInput = Input.GetAxisRaw("Vertical");
        if (verticalInput != lastVerticalInput)
        {
            //reset hidecountdown on input
            countDown = 0;
            if (verticalInput > 0)
            {
                currentIndex++;
                if (currentIndex == emoteSprites.Length)
                    currentIndex = 0;

                isForward = true;
                ShowWheel(currentIndex);
            }
            if (verticalInput < 0)
            {
                currentIndex--;
                if (currentIndex < 0)
                    currentIndex += emoteSprites.Length;
                ShowWheel(currentIndex);
            }

            lastVerticalInput = verticalInput;
        }
        //count up hidecountdown
        countDown += Time.deltaTime;
    }

    private void ShowWheel(int index)
    {
        shownEmotes = new List<Sprite>();
        int currentSprite = index;

        //only ever show 3
        for (int i = 0; i < 3; i++)
        {
            if (currentSprite < 0)
            {
                currentSprite = emoteSprites.Length - 1;
                //invert
                currentIndex = emoteSprites.Length - 1;
            }

            //Middle Emote is always the one that will fire
            if (i == 1)
            {
                currentSelectedEmote = currentSprite;
                emoteChosen = true;
            }

            shownEmotes.Add(emoteSprites[currentSprite]);
            currentSprite++;

            //reset if next sprite is out of bounds
            if (currentSprite >= emoteSprites.Length)
                currentSprite = 0;
        }

        var y = 0;
        foreach (var spr in shownEmotes)
        {
            var wheelItem = wheelItems[y];
            wheelItem.GetComponent<SpriteRenderer>().sprite = spr;
            y++;
        }
    }
}
