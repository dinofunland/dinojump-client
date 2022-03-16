using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    private Rigidbody2D rb2d;

    void Go()
    {
        float random = Random.Range(0, 2);
        if(random < 1)
        {
            rb2d.AddForce(new Vector2(Random.Range(90, 110), -15));
        } else {
            rb2d.AddForce(new Vector2(-200, -15));
        }

        float randomFacing = Random.Range(0, 360);
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, randomFacing, transform.rotation.w);
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Go();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb2d.velocity.x == 0)
        {
            rb2d.velocity = new Vector2(Random.Range(1, 3), rb2d.velocity.y);
        } else if (rb2d.velocity.y == 0) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Random.Range(1, 3));
        }
    }
}
