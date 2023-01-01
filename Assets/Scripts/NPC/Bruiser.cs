using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : Enemy
{
    private Animator animator;
    private Rigidbody2D rb;

    public float moveSpeed;
    public float knockbackTaken;
    public float knockHeight;
    private bool move;
    private bool hit;
    private bool attacking;
    private Player playerClass;
    private Enemy enemy;
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
        if ((!canSeePlayer && !canSeeEnemy) || rb.velocity == Vector2.zero || hit)
            animator.SetBool("Moving", false);
        else
            animator.SetBool("Moving", true);

        if (health <= 0)
        {
            animator.SetTrigger("Die");
            gameObject.GetComponent<Bruiser>().enabled = false;
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
        else if(canSeeEnemy && move && !hit)
            rb.velocity = new Vector2(enemyDirection.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        else if (!hit || attacking)
            rb.velocity = new Vector2(0, rb.velocity.y);

        if (rb.velocity.x < 0 && !hit)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else if (rb.velocity.x > 0 && !hit)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !attacking)
        {
            animator.SetBool("Attacking", true);
            playerClass = collision.gameObject.GetComponent<Player>();
            StartCoroutine(Reset());
        }
        else if(collision.gameObject.CompareTag("Neuvistran") && !attacking)
        {
            animator.SetBool("Attacking", true);
            enemy = collision.gameObject.GetComponent<Enemy>();
            StartCoroutine(Reset());
        }
        if (collision.gameObject.CompareTag("Player") && (Mathf.Abs(transform.position.x - collision.gameObject.transform.position.x) < 3f))
             playerClass.Hit(10f, 0.75f, direction, 10f, transform.position);
    }

    public void Attack()
    {
        if (Physics2D.IsTouching(player.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>()))
            playerClass.Hit(10f, 0.75f, direction, 25f, transform.position);
        else if (Physics2D.IsTouching(enemy.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>()))
            enemy.Hit(1f, enemyDirection);
    }

    IEnumerator Reset()
    {
        move = false;
        attacking = true;
        yield return new WaitForSeconds(1f);
        move = true;
        attacking = false;
    }

    public void StopAttack()
    {
        animator.SetBool("Attacking", false);
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
