using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField]
    GameObject background;
    private Camera mainCamera;
    private Vector2 screenBounds;
    public float choke;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        LoadChildObjects(background);
    }

    void LoadChildObjects(GameObject obj)
    {
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x -choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objectWidth);
        GameObject clone = Instantiate(obj);
        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone);
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + i;
            //LoadChildObjectsHeight(c);
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RepositionChildObjects(background);
    }

    private void RepositionChildObjects(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1) 
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[^1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x -choke;

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

            //foreach (var child in children)
            //{
            //    RepositionChildObjectsHeight(child.gameObject);
            //}
        }
    }

    void LoadChildObjectsHeight(GameObject obj)
    {
        float objectHeight = obj.GetComponent<SpriteRenderer>().bounds.size.y - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.y * 2 / objectHeight);
        GameObject clone = Instantiate(obj);
        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone);
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectHeight * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + "_y_"+ i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    private void RepositionChildObjectsHeight(GameObject obj)
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
                firstChild.transform.position = new Vector3(lastChild.transform.position.y + halfObjectHeight * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if (mainCamera.gameObject.transform.position.y - screenBounds.y < firstChild.transform.position.y - halfObjectHeight)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.y - halfObjectHeight * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }
}
