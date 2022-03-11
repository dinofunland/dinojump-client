using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaLoop : MonoBehaviour
{
    [SerializeField]
    GameObject lavaContainer;
    private Camera mainCamera;
    private Vector2 screenBounds;
    public float choke;

    private float halfObjectWidth;
    private float halfObjectHeight;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        LoadFirstRow(lavaContainer);
    }

    private void LateUpdate()
    {
        RepositionInsideRow(lavaContainer);
    }

    private void RepositionInsideRow(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[^1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;

            if (mainCamera.gameObject.transform.position.x + screenBounds.x > lastChild.transform.position.x + halfObjectWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if (mainCamera.gameObject.transform.position.x - screenBounds.x < firstChild.transform.position.x - halfObjectWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }

    void LoadFirstRow(GameObject container)
    {
        float objectWidth = container.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objectWidth);
        GameObject clone = Instantiate(container);
        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone);
            c.transform.SetParent(container.transform);
            c.transform.position = new Vector3(objectWidth * i, container.transform.position.y, container.transform.position.z);
            c.name = "LavaTile" + i;
            Destroy(c.GetComponent<LavaController>());
        }
        Destroy(clone);
        Destroy(container.GetComponent<SpriteRenderer>());
        Destroy(container.GetComponent<Animator>());
    }
}
