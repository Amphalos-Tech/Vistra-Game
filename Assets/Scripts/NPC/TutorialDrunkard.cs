using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDrunkard : Enemy
{
    private Animator animator;
    private Rigidbody2D rb;

    public float moveSpeed;
    private bool move;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        base.Exist();
        move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSeePlayer || rb.velocity == Vector2.zero)
            animator.SetBool("Moving", false);
        else
            animator.SetBool("Moving", true);
    }

    void FixedUpdate()
    {
        if (canSeePlayer && move)
            rb.velocity = new Vector2(direction.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        else
            rb.velocity = Vector2.zero;

        if (rb.velocity.x < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else if (rb.velocity.x > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Hit(5f, 0.75f, direction, 10f, transform.position);
            StartCoroutine(Reset());
        }
    }

    IEnumerator Reset()
    {
        move = false;
        yield return new WaitForSeconds(1f);
        move = true;
    }
}
