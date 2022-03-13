using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSplashController : MonoBehaviour
{
    public float offset;
    public AudioSource dropSound;
    // Start is called before the first frame update
    void Start()
    {
        var newTransform = transform.position;
        newTransform.y += offset;
        this.transform.position = newTransform;
    }
    void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
