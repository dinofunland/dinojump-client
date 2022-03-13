using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSplashController : MonoBehaviour
{
    Animator splashAnimator;
    Animation splash;
    // Start is called before the first frame update
    void Start()
    {
        splashAnimator = GetComponent<Animator>();
        splashAnimator.fireEvents = true;
        splashAnimator.Play("LavaSplash");
    }

    void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
