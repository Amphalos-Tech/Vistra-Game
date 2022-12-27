using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public float fallSpeed;
    public float lowFall;
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
    private bool hasJumped;
    private bool isDashing;
    private bool wallOnLeft;
    [SerializeField] private bool canWallJump;

    // Start is called before the first frame update
    void Start()
    {
        canDoubleJump = true;
        isDashing = false;
        dashCooldownCount = 0;
        Physics.gravity = new Vector3(0, -9.8f, 0);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  
    }
    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowFall - 1) * Time.deltaTime;
            animator.SetBool("isFalling", false);
        } else
            animator.SetBool("isFalling", false);

        IsOnGround();
        animator.SetBool("isgrounded", canJump);
        animator.SetBool("isDashing", isDashing);
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump)
                canDoubleJump = true;

            if ((canDoubleJump || canJump) && !IsOnWall())
                Jump(1f);
            if (hasJumped && canDoubleJump)
                canDoubleJump = false;
            if (canDoubleJump && !canJump && Input.GetButtonUp("Jump"))
            {
                hasJumped = true;
            }
        }
        if (IsOnWall() && (wallOnLeft ? moveDirection.x > 0 : moveDirection.x < 0) && Input.GetButton("Jump") && canWallJump && !canJump)
        {
            Jump(0.75f);
            canDoubleJump = false;
            canWallJump = false;
        }
        if (IsOnWall() && rb.velocity == Vector2.zero)
            canWallJump = true;

        if (dashCooldownCount > 0)
            dashCooldownCount -= Time.deltaTime;

        if(Input.GetButtonDown("Dash") && dashCooldownCount <= 0 && moveDirection.x != 0)
        {
            dashCooldownCount = dashCooldown;
            isDashing = true;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        if (rb.velocity.x < 0)
        {
            if(canJump)
                animator.SetBool("Moving", true);
            else
                animator.SetBool("Moving", false);
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (rb.velocity.x > 0)
        {
            if (canJump)
                animator.SetBool("Moving", true);
            else
                animator.SetBool("Moving", false);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
            animator.SetBool("Moving", false);

        if (isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    public void Jump(float divider)
    {
        if (divider == 1f)
            animator.SetBool("isJumping", true);
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpSpeed/divider);
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
