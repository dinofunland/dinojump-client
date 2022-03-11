using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dinojump.Schemas;

public class LavaController : MonoBehaviour
{
    public FloorSchema floorSchema { get; set; }
    float lerpSpeed = 0.02f;

    float spriteSize;
    float offset =  2;
    // Start is called before the first frame update
    void Start()
    {
        spriteSize = GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (floorSchema?.position != null)
        {
            var yPosOffset = spriteSize - offset;
            var t = Time.deltaTime / lerpSpeed;
            Vector2 desiredPostion = new Vector3(floorSchema.position.x, floorSchema.position.y - yPosOffset);
            transform.position = Vector2.Lerp(transform.position, desiredPostion, t);
        }
    }

    private void OnDrawGizmos()
    {
        if (floorSchema != null)
        {
            Gizmos.color = Color.red;
            Vector3 left = new Vector3(-500, floorSchema.position.y);
            Vector3 right = new Vector3(500, floorSchema.position.y);
            Gizmos.DrawLine(left, right);
        }
    }
}
