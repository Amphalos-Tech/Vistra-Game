using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBomber : Enemy
{
    public float knockbackTaken;
    public float knockHeight;
    public GameObject bullet;
    public Vector2 projectileOffset;

    private int offset;
    private float animationOffset = 0.66f;
    private bool attacking;
    private bool playerInRange;
    private Animator animator;
    private Rigidbody2D rb;
    private bool hit;
    private Vector2 enemyDirection;
    private bool canSeeEnemy;
    private bool enemyInRange;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        base.Exist();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        animator.SetBool("IsInRange", canSeePlayer);

        if (canSeePlayer)
        {
            if (transform.position.x > player.transform.position.x && !hit)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                offset = -1;
            }
            else if (!hit)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                offset = 1;
            }
        }


        if (health <= 0)
        {
            //animator.SetTrigger("Die");
            gameObject.GetComponent<BoxBomber>().enabled = false;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        attacking = true;
        if (canSeePlayer)
        {
            while (canSeePlayer)
            {
                yield return new WaitForSeconds(1f);
                if (hit)
                    yield break;
                animator.SetBool("Throw", true);
            }
            animator.SetBool("Throw", false);
        }
        attacking = false;
    }


    


    public void Throw()
    {
        Instantiate(bullet, new Vector2(transform.position.x + offset * projectileOffset.x, transform.position.y + projectileOffset.y), transform.localRotation);
    }

    public void StopAttack()
    {
        animator.SetBool("Throw", false);
    }

    public override void Hit(float damage, Vector2 d)
    {
        base.Hit(damage, d);

        if (canSeePlayer)
            rb.velocity = new Vector2(d.x * knockbackTaken, rb.velocity.y + knockHeight);
        else
            rb.velocity = new Vector2(d.x * knockbackTaken / 2, rb.velocity.y + knockHeight);
        hit = true;
        StartCoroutine(ColorIndicator());
    }

    IEnumerator ColorIndicator()
    {
        if (canSeePlayer)
            animator.SetBool("Attack", false);
        GetComponent<SpriteRenderer>().color = new Color(1, 0.675f, 0.675f);
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        hit = false;
    }
}
