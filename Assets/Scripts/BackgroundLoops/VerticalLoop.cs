using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalLoop : MonoBehaviour
{
    [SerializeField]
    List<GameObject> obtjectsToLoop;
    private Camera mainCamera;
    private Vector2 screenBounds;
    public float choke;

    private float halfObjectWidth;
    private float halfObjectHeight;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        foreach (var item in obtjectsToLoop)
        {
            LoadFirstRow(item);
        }
    }

    private void LateUpdate()
    {
        foreach (var item in obtjectsToLoop)
        {
            RepositionInsideRow(item);
        }
    }

    private void RepositionInsideRow(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[^1].gameObject;
            float halfObjectHeight = lastChild.GetComponent<SpriteRenderer>().bounds.extents.y - choke;

            if (mainCamera.gameObject.transform.position.y + screenBounds.y > lastChild.transform.position.y + halfObjectHeight)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x, lastChild.transform.position.y + halfObjectHeight * 2, lastChild.transform.position.z);
            }
            else if (mainCamera.gameObject.transform.position.y - screenBounds.y < firstChild.transform.position.y - halfObjectWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x, firstChild.transform.position.y - halfObjectHeight * 2, firstChild.transform.position.z);
            }
        }
    }

    void LoadFirstRow(GameObject container)
    {
        float objectHeight = container.GetComponent<SpriteRenderer>().bounds.size.y - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.y * 2 / objectHeight);
        GameObject clone = Instantiate(container);
        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone);
            c.transform.SetParent(container.transform);
            c.transform.position = new Vector3(container.transform.position.x, objectHeight * i, container.transform.position.z);
            c.name = clone.name + i;
            Destroy(c.GetComponent<LavaController>());
        }
        Destroy(clone);
        Destroy(container.GetComponent<SpriteRenderer>());
        Destroy(container.GetComponent<Animator>());
    }
}
