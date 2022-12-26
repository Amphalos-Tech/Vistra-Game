using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public bool canJump;
    public bool canDoubleJump;
    public LayerMask groundLayer;

    public float dashSpeed;
    public float dashCooldown;
    private float dashCooldownCount;
    private bool canDash => dashCooldownCount > 0;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isDashing;
    private bool wallOnLeft;
    [SerializeField] private bool canWallJump;

    // Start is called before the first frame update
    void Start()
    {
        canDoubleJump = true;
        dashCooldownCount = 0;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(IsOnWall());
        IsOnGround();
        animator.SetBool("isgrounded", canJump);
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump)
                canDoubleJump = true;
            if(IsOnWall())
                canWallJump = true;
            if (canDoubleJump || canJump || canWallJump)
                StartCoroutine(Jump());
            if (canDoubleJump && !canJump)
                canDoubleJump = false;
            canWallJump = false;
        }

        if (dashCooldownCount > 0)
            dashCooldownCount -= Time.deltaTime;

        if(Input.GetButtonDown("Dash") && dashCooldownCount <= 0)
        {
            dashCooldownCount = dashCooldown;
            isDashing = true;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime, rb.velocity.y);
        if (rb.velocity.x < 0)
        {
            animator.SetBool("Moving", true);
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (rb.velocity.x > 0)
        {
            animator.SetBool("Moving", true);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
            animator.SetBool("Moving", false);

        if (isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    public IEnumerator Jump()
    {
        bool jumping = false;
        if(canJump || canDoubleJump)
            jumping = true;
        if (canWallJump)
        {
            jumping = true;
            if (rb.velocity == Vector2.zero || !(wallOnLeft ? moveDirection.x > 0 : moveDirection.x < 0)) 
            {
                Debug.Log("G");
                jumping = false;
            }
        }

        if (jumping)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpSpeed * Time.deltaTime);



        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator Dash()
    {
        rb.AddForce(new Vector2(dashSpeed * moveDirection.x * Time.deltaTime, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.15f);
        isDashing = false;
    }

    public void IsOnGround()
    {
        canJump = Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
    }

    public bool IsOnWall()
    {
        if (!canJump)
        {
            if(Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.right, 0.15f, groundLayer))
            {
                wallOnLeft = false;
                return true;
            } else if(Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.left, 0.15f, groundLayer))
            {
                wallOnLeft = true;
                return true;
            }
        }
        return false;
    }
}
