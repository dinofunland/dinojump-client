using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField]
    private float moveSpeed = 1f;
    float xBounds = 80f;
    float yBounds = 35f;
    void Go()
    {
        float randomFacing = Random.Range(1, 360);
        gameObject.transform.Rotate(Vector3.forward, randomFacing);

        float randomY = Random.Range(-yBounds, yBounds);
        float randomX = Random.Range(-xBounds, xBounds);
        Vector2 position = new Vector2(randomX, randomY);
        gameObject.transform.position = position;
        rb2d.AddForce(-gameObject.transform.right * (moveSpeed * 10));
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Go();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.AddForce(-gameObject.transform.right * moveSpeed);
        //if(rb2d.velocity.x == 0)
        //{
        //    rb2d.velocity = new Vector2(Random.Range(1, 3), rb2d.velocity.y);
        //} else if (rb2d.velocity.y == 0) {
        //    rb2d.velocity = new Vector2(rb2d.velocity.x, Random.Range(1, 3));
        //}
    }
}
