using Dinojump.Schemas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : MonoBehaviour
{
    public PlatformSchema PlatformSchema;
    public string Key;
    [SerializeField]
    float lerpSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        if (PlatformSchema.type == "Moving")
        {
            var newPos = new Vector2(PlatformSchema.position.x, PlatformSchema.position.y);
            transform.position = Vector2.Lerp(transform.position, newPos, lerpSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 size = new Vector3(PlatformSchema.size.width, PlatformSchema.size.height);
        Gizmos.DrawWireCube(new Vector3(PlatformSchema.position.x, PlatformSchema.position.y), size);
    }
}
