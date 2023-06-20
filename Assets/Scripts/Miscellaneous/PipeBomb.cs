using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBomb : MonoBehaviour
{

    public float speed;
    public float damage;
    public float iframes;
    public float knockback;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public GameObject explosion;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (transform.rotation == Quaternion.Euler(0, 0, 0))
            rb.AddForce(new Vector2(speed * 2, speed), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(-speed * 2, speed), ForceMode2D.Impulse);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 direction;
        if (transform.position.x < collision.gameObject.transform.position.x)
            direction = new Vector2(1, 0);
        else
            direction = new Vector2(-1, 0);
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject particles = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(particles, 0.4f);
            collision.gameObject.GetComponent<Player>().Hit(damage, iframes, direction, knockback, transform.position);
            Destroy(gameObject);
        } else if(collision.gameObject.CompareTag("Ground"))
        {
            GameObject particles = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(particles, 0.4f);
            Collider2D playerOverlap = Physics2D.OverlapCircle(transform.position, 3f, playerLayer);
            if(playerOverlap != null)
            {
               playerOverlap.gameObject.GetComponent<Player>().Hit(damage, iframes, direction, knockback, transform.position);
            }
            Destroy(gameObject);
        }
    }

    public void SpeedChange(bool unhit)
    {
        Debug.Log("Game");
        if (unhit)
            speed *= 4;
        else
            speed /= 4;
    }
}
