using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dinojump.Schemas;

public class LavaController : MonoBehaviour
{
    public FloorSchema floorSchema { get; set; }
    float lerpSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (floorSchema?.position != null)
        {
            var t = Time.deltaTime / lerpSpeed;
            Vector2 desiredPostion = new Vector3(floorSchema.position.x, floorSchema.position.y);
            transform.position = Vector2.Lerp(transform.position, desiredPostion, t);
        }
    }

    private void OnDrawGizmos()
    {
        if (floorSchema != null)
        {
            Gizmos.color = Color.red;
            Vector3 size = new Vector3(floorSchema.size.width, floorSchema.size.height);
            Gizmos.DrawWireCube(this.gameObject.transform.position, size);
        }
    }
}
