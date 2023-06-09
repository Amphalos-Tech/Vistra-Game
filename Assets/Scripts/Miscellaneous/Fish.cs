using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float speed;
    public float damage;
    public float iframes;
    public float knockback;
    public LayerMask groundLayer;
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
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("National Party"))
        {
            Vector2 direction;
            if (transform.position.x < collision.gameObject.transform.position.x)
                direction = new Vector2(1, 0);
            else
                direction = new Vector2(-1, 0);

            collision.gameObject.GetComponent<Enemy>().Hit(0, direction);
            Destroy(gameObject);
        } else if(collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(Flop());
        }
    }


    IEnumerator Flop()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
