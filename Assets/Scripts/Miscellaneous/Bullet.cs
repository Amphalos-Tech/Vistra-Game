using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float iframes;
    public float knockback;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (transform.rotation == Quaternion.Euler(0, 0, 0))
            rb.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(-speed, 0), ForceMode2D.Impulse);
        
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direction;
            if (transform.position.x < collision.gameObject.transform.position.x)
                direction = new Vector2(1, 0);
            else
                direction = new Vector2(-1, 0);
                
            collision.gameObject.GetComponent<Player>().Hit(damage, iframes, direction, knockback, transform.position);
        }
        Destroy(gameObject);
    }

}
