using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 direction;
            if (transform.position.x < collision.gameObject.transform.position.x)
                direction = new Vector2(1, 0);
            else
                direction = new Vector2(-1, 0);

            collision.gameObject.GetComponent<Enemy>().Hit(damage, direction);
        }
        Destroy(gameObject);
    }
}
