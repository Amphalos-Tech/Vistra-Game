using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    public float range;
    public float rangeHeight;
    public float moveSpeed;
    public float knockbackTaken;
    public float knockHeight;
    public GameObject bullet;

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
        if (canSeePlayer || canSeeEnemy)
            StartCoroutine(RangeChecker());
        else if (!canSeePlayer)
            StartCoroutine(EnemyChecker());
    }

    private void FixedUpdate()
    {
        if (!playerInRange && !hit && canSeePlayer)
        {
            animator.SetBool("Moving", true);
            rb.velocity = new Vector2(direction.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else if (!attacking && !hit && playerInRange)
        {
            animator.SetBool("Moving", false);
            if (animationOffset > 0f)
                animationOffset -= Time.deltaTime;
            else
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("Attack", true);
                StartCoroutine(Attack());
            }

        }
        else if (canSeeEnemy && !hit && !attacking)
        {
            animator.SetBool("Moving", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("Attack", true);
            StartCoroutine(Attack());
        }
        else
            animator.SetBool("Moving", false);


        if(canSeePlayer)
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
        } else if(canSeeEnemy)
        {
            if(enemyDirection.x < 0 && !hit)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                offset = -1;
            } else if(enemyDirection.x > 0 && !hit)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                offset = 1;
            }
        }


        if (health <= 0)
        {
            animator.SetTrigger("Die");
            gameObject.GetComponent<Gunner>().enabled = false;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        attacking = true;
        if (playerInRange)
        {
            while (playerInRange)
            {
                yield return new WaitForSeconds(1f);
                if (hit)
                    yield break;
                animator.SetBool("Attack", true);
            }
        }
        else if (enemyInRange)
        {
            while (enemyInRange)
            {
                yield return new WaitForSeconds(1f);
                animator.SetBool("Attack", true);
            }
        }
        attacking = false;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawLine(new Vector2(transform.position.x + 5, transform.position.y), new Vector2(transform.position.x + 5, transform.position.y + rangeHeight));
        Gizmos.DrawLine(new Vector2(transform.position.x + 5, transform.position.y), new Vector2(transform.position.x + 5, transform.position.y - rangeHeight));
    }

    IEnumerator RangeChecker()
    {
        if (canSeePlayer)
        {
            while (canSeePlayer)
            {
                yield return new WaitForSeconds(delay);
                if (!player.activeSelf)
                    player = GameObject.FindWithTag("Player");
                CheckRange();
            }
        }
        else if (canSeeEnemy)
        {
            while (canSeeEnemy)
            {
                yield return new WaitForSeconds(delay);
                CheckRange();
            }
        }
    }

    IEnumerator EnemyChecker()
    {
        WaitForSeconds delaySeconds = new WaitForSeconds(delay);

        while (true)
        {
            yield return delaySeconds;
            if (!player.activeSelf)
                player = GameObject.FindWithTag("Player");
            PositionCheck();
        }
    }

    void CheckRange()
    {
        if (canSeePlayer)
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
        else if (canSeeEnemy)
        {
            Collider2D rangeObj = Physics2D.OverlapCircle(transform.position, range, enemyLayer);
            if (rangeObj != null && Mathf.Abs(rangeObj.transform.position.y - transform.position.y) <= rangeHeight)
            {
                enemyDirection = new Vector2(rangeObj.transform.position.x - transform.position.x, rangeObj.transform.position.y - transform.position.y).normalized;
                if (Vector2.Angle(transform.position, direction) < angle / 2)
                {
                    float distance = Vector2.Distance(transform.position, rangeObj.transform.position);

                    if (Physics2D.Raycast(transform.position, enemyDirection, distance, obstruction))
                        enemyInRange = false;
                    else
                        enemyInRange = true;
                }
                else
                    enemyInRange = false;
            }
            else
                enemyInRange = false;
        }
    }

    void PositionCheck()
    {

        RaycastHit2D rangeObj = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(1, 0), radius, enemyLayer);
        if (rangeObj == false)
            rangeObj = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(-1, 0), radius, enemyLayer);

        if (rangeObj && !rangeObj.transform.gameObject.CompareTag(gameObject.tag) && Mathf.Abs(rangeObj.transform.position.y - transform.position.y) <= height)
        {
            enemyDirection = new Vector2(rangeObj.transform.position.x - transform.position.x, rangeObj.transform.position.y - transform.position.y).normalized;
            if (Vector2.Angle(transform.position, direction) < angle / 2)
            {
                float distance = Vector2.Distance(transform.position, rangeObj.transform.position);

                if (Physics2D.Raycast(transform.position, enemyDirection, distance, obstruction))
                    canSeeEnemy = false;
                else
                    canSeeEnemy = true;
            }
            else
                canSeeEnemy = false;
        }
        else
            canSeeEnemy = false;
    }



    public void Shoot()
    {
        Instantiate(bullet, new Vector2(transform.position.x + offset * 3f, transform.position.y), transform.localRotation);
    }

    public void StopAttack()
    {
        animator.SetBool("Attack", false);
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
        animator.SetBool("Attack", false);
        GetComponent<SpriteRenderer>().color = new Color(1, 0.675f, 0.675f);
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        hit = false;
    }
}
