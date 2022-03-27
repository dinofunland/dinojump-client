using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteController : MonoBehaviour
{
    [SerializeField] Sprite cryEmote;
    [SerializeField] Sprite laughEmote;
    [SerializeField] Sprite heartEmote;
    [SerializeField] Sprite thumbsupEmote;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void TriggerEmote(int type)
    {

        switch ((EmoteType)type)
        {
            case EmoteType.Cry:
                spriteRenderer.sprite = cryEmote;
                break;
            case EmoteType.Laugh:
                spriteRenderer.sprite = laughEmote;
                break;
            case EmoteType.Heart:
                spriteRenderer.sprite = heartEmote;
                break;
            case EmoteType.ThumbsUp:
                spriteRenderer.sprite = thumbsupEmote;
                break;
        }

        spriteRenderer.enabled = true;
        StartCoroutine(Wait());
    }

    enum EmoteType
    {
        ThumbsUp = 0,
        Cry = 1,
        Laugh = 2,
        Heart = 3,
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        spriteRenderer.enabled = false;
    }
}
