using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowardGunner : Enemy
{
    public float range;
    public float rangeHeight;
    public float moveSpeed;
    public float knockbackTaken;
    public float knockHeight;
    public GameObject bullet;

    public float closeRange;

    private int offset;
    private bool attacking;
    private bool playerInRange;
    private bool playerTooClose;
    private Animator animator;
    private Rigidbody2D rb;
    private bool hit;
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
        if (canSeePlayer)
            StartCoroutine(RangeChecker());
    }

    private void FixedUpdate()
    {
        if (!playerInRange && !hit && canSeePlayer)
        {
            animator.SetBool("Moving", true);
            rb.velocity = new Vector2(direction.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (!attacking && !hit && playerInRange && !playerTooClose)
        {
            animator.SetBool("Moving", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetTrigger("Attack");
            StartCoroutine(Attack());

        } else if(playerTooClose && !hit)
        {
            animator.SetBool("Moving", true);
            rb.velocity = new Vector2(-direction.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if(!hit)
        {
            animator.SetBool("Moving", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        } else
            animator.SetBool("Moving", false);


        if (playerInRange)
            StartCoroutine(CheckClose());


        if (transform.position.x > player.transform.position.x && !hit && !playerTooClose)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            offset = -1;
        }
        else if(transform.position.x < player.transform.position.x && !hit && !playerTooClose) {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            offset = 1;
        }
        else if (!hit && playerTooClose && transform.position.x > player.transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            offset = 1;
        } else if(!hit && playerTooClose && transform.position.x < player.transform.position.x)
        {
            offset = -1;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }



        if (health <= 0)
        {
            animator.SetTrigger("Die");
            gameObject.GetComponent<CowardGunner>().enabled = false;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        attacking = true;
        while (playerInRange)
        {
            yield return new WaitForSeconds(1f);
            animator.SetTrigger("Attack");
        }
        attacking = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Hit(5f, 0.75f, direction, 15f, transform.position);
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireSphere(transform.position, closeRange);
        Gizmos.DrawLine(new Vector2(transform.position.x + 5, transform.position.y), new Vector2(transform.position.x + 5, transform.position.y + rangeHeight));
        Gizmos.DrawLine(new Vector2(transform.position.x + 5, transform.position.y), new Vector2(transform.position.x + 5, transform.position.y - rangeHeight));
    }

    IEnumerator RangeChecker()
    {
        while (canSeePlayer)
        {
            yield return new WaitForSeconds(delay);
            if (!player.activeSelf)
                player = GameObject.FindWithTag("Player");
            CheckRange();
        }
    }

    IEnumerator CheckClose()
    {
        while(playerInRange)
        {
            yield return new WaitForSeconds(delay);
            if (!player.activeSelf)
                player = GameObject.FindWithTag("Player");
            CheckCloseRange();
        }
    }

    void CheckCloseRange()
    {
        Collider2D rangeObj = Physics2D.OverlapCircle(transform.position, closeRange, playerLayer);
        if (rangeObj != null && Mathf.Abs(rangeObj.transform.position.y - transform.position.y) <= rangeHeight)
        {
            direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized;
            if (Vector2.Angle(transform.position, direction) < angle / 2)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);

                if (Physics2D.Raycast(transform.position, direction, distance, obstruction))
                    playerTooClose = false;
                else
                    playerTooClose = true;
            }
            else
                playerTooClose = false;
        }
        else
            playerTooClose = false;
    }

    void CheckRange()
    {
        Collider2D rangeObj = Physics2D.OverlapCircle(transform.position, range, playerLayer);
        if (rangeObj != null && Mathf.Abs(rangeObj.transform.position.y - transform.position.y) <= rangeHeight)
        {
            direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized;
            if (Vector2.Angle(transform.position, direction) < angle / 2)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);

                if (Physics2D.Raycast(transform.position, direction, distance, obstruction))
                    playerInRange = false;
                else
                    playerInRange = true;
            }
            else
                playerInRange = false;
        }
        else
            playerInRange = false;
    }



    public void Shoot()
    {
        Instantiate(bullet, new Vector2(transform.position.x + offset * 3f, transform.position.y), transform.localRotation);
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
        yield return new WaitForSeconds(0.75f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        hit = false;
    }
}
