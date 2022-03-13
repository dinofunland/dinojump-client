using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dinojump.Schemas;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> platformPrefabs;

    [SerializeField]
    float lerpSpeed;

    float platformYOffset = 1.2f;
    public void SpawnPlatform(string key, PlatformSchema platSchema)
    {
        //Debug.Log("SpawnManager.SpawnPlatform! Type: " + platSchema.type);
        GameObject prefab;
        switch (platSchema.type)
        {
            case "Falling":
                prefab = platformPrefabs.FirstOrDefault(p => p.name.Contains("Falling"));
                break;
            case "Moving":
                prefab = platformPrefabs.FirstOrDefault(p => p.name.Contains("Moving"));
                break;
            default:
                prefab = platformPrefabs.FirstOrDefault(p => p.name.Contains("Static")); 
                break;
        }
        var platform = Instantiate(prefab, new Vector2(platSchema.position.x,platSchema.position.y + platformYOffset), prefab.transform.rotation);
        platform.GetComponent<PlatformBase>().PlatformSchema = platSchema;
        platform.GetComponent<PlatformBase>().Key = key;

    }

    public void RemovePlatform(string key)
    {
        Destroy(FindObjectsOfType<PlatformBase>().FirstOrDefault(p => p.Key == key).gameObject);
    }

    enum PlatformType
    {
        STATIC = 'S',
        FALLING = 'F',
        MOVING = 'M'
    }

    internal void UpdatePlatform(string key, PlatformSchema value)
    {
        var platform = FindObjectsOfType<PlatformBase>().FirstOrDefault(p => p.Key == key).gameObject;
        Vector3.Lerp(platform.transform.position, new Vector2(value.position.x, value.position.y), Time.deltaTime * lerpSpeed);
        platform.GetComponent<PlatformBase>().PlatformSchema = value;
    }
}
