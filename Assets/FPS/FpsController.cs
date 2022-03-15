using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FpsController : MonoBehaviour
{
    public float pollingTime = 1f;
    public bool showFpsCounter = false;
    private float time;
    private int frameCount;

    private UIDocument fpsUIDocument;

    void Start()
    {
        fpsUIDocument = GetComponent<UIDocument>();
    }

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;

        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsUIDocument.rootVisualElement.Q<Label>().text = frameRate.ToString() + " FPS";
        }

        fpsUIDocument.rootVisualElement.style.display = showFpsCounter ? DisplayStyle.Flex : DisplayStyle.None;

        if (Input.GetKeyDown(KeyCode.F))
        {
            showFpsCounter = !showFpsCounter;
        }
    }
}
