using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDrunkard : Enemy
{
    private Animator animator;
    private Rigidbody2D rb;

    public float moveSpeed;
    public float knockbackTaken;
    public float knockHeight;
    private bool move;
    private bool hit;
    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        base.Exist();
        move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSeePlayer || rb.velocity == Vector2.zero || hit)
            animator.SetBool("Moving", false);
        else
            animator.SetBool("Moving", true);

        if(health <= 0)
        {
            animator.SetTrigger("Die");
            gameObject.GetComponent<TutorialDrunkard>().enabled = false;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }


    void FixedUpdate()
    {
        if (canSeePlayer && move && !hit)
            rb.velocity = new Vector2(direction.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        else if (!hit)
            rb.velocity = new Vector2(0, rb.velocity.y);

        if (rb.velocity.x < 0 && !hit)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else if (rb.velocity.x > 0 && !hit)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Hit(5f, 0.75f, direction, 15f, transform.position);
            StartCoroutine(Reset());
        }
    }

    IEnumerator Reset()
    {
        move = false;
        yield return new WaitForSeconds(1f);
        move = true;
    }

    public override void Hit(float damage, Vector2 d)
    {
        base.Hit(damage, d);

        rb.velocity = new Vector2(d.x * knockbackTaken, rb.velocity.y + knockHeight);
        hit = true;
        StartCoroutine(ColorIndicator());
    }

    IEnumerator ColorIndicator()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0.675f, 0.675f);
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        hit = false;
    }

    
}
