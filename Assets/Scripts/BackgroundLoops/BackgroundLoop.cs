using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField]
    GameObject background;
    [SerializeField]
    GameObject bgContainer;
    private Camera mainCamera;
    private Vector2 screenBounds;
    public float choke;

    private float halfObjectWidth;
    private float halfObjectHeight;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        var bgRow = LoadFirstRow(background);
        bgRow.transform.SetParent(bgContainer.transform);

        LoadAdditionalRows(bgRow);
    }

    private void LoadAdditionalRows(GameObject bgRow)
    {
        float objectHeight = bgRow.GetComponent<SpriteRenderer>().bounds.size.y - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.y * 4 / objectHeight);
        GameObject rowClone = Instantiate(bgRow);
        for (int i = 0; i <= childsNeeded; i++)
        {
            
            GameObject c = Instantiate(rowClone);
            var pos = bgContainer.transform.GetChild(0).position.x;
            c.transform.SetParent(bgContainer.transform);
            c.transform.position = new Vector3(pos, objectHeight * i, bgRow.transform.position.z);
            c.name = "BGRow" + i;

        }
        Destroy(rowClone);
        foreach (var childRenderer in bgRow.GetComponentsInChildren<SpriteRenderer>())
        {
            Destroy(childRenderer);
        }
        Destroy(GameObject.Find("BGRowOG"));
    }

    GameObject LoadFirstRow(GameObject bgRow)
    {
        float objectWidth = bgRow.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 4 / objectWidth);
        GameObject clone = Instantiate(bgRow);
        bgRow.name = "BGRowOG";
        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone);
            c.transform.SetParent(bgRow.transform);
            c.transform.position = new Vector3(objectWidth * i, bgRow.transform.position.y, bgRow.transform.position.z);
            c.name = "BGTile" + i;
        }
        Destroy(clone);
        Destroy(bgRow.GetComponent<SpriteRenderer>());

        return bgRow;
    }

    void LateUpdate()
    {
        var directChildren = bgContainer.GetComponentsInChildren<Transform>().Where(m => m.name.Contains("BGRow")).ToList();
        foreach (var child in directChildren)
        {
            RepositionInsideRow(child.gameObject);
        }
            
        RepositionInsideContainer(bgContainer);
    }

    private void RepositionInsideContainer(GameObject bgContainer)
    {
        //only rows from children
        Transform[] children = bgContainer.GetComponentsInChildren<Transform>().Where(m => m.name.Contains("BGRow")).ToArray();

        if (children.Length > 1)
        {
            GameObject firstChild = children[0].gameObject;
            GameObject lastChild = children[^1].gameObject;
            float halfObjectHeight = lastChild.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.y - choke;

            if (mainCamera.gameObject.transform.position.y + screenBounds.y > lastChild.transform.position.y + halfObjectHeight)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x, lastChild.transform.position.y + halfObjectHeight * 2, lastChild.transform.position.z);
            }
            else if (mainCamera.gameObject.transform.position.y - screenBounds.y < firstChild.transform.position.y - halfObjectHeight)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x, firstChild.transform.position.y - halfObjectHeight * 2, firstChild.transform.position.z);
            }
        }
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

}
