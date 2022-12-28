using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public float fallSpeed;
    public float lowFall;
    public bool invincible;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    public float dashSpeed;
    public float dashCooldown;
    private float dashCooldownCount;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool canJump;
    private bool canDoubleJump;
    private bool wallOnLeft;
    private bool canWallJump;
    private GameObject oneWayPlatform;
    private static byte[] upgrades;
    private static bool loadedUpgrades = false;

    // Start is called before the first frame update
    void Start()
    {
        canDoubleJump = true;
        dashCooldownCount = 0;
        Physics.gravity = new Vector3(0, -9.8f, 0);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        
        if(!loadedUpgrades)
        {
            //upgrades = SaveHandler.Upgrades;
            loadedUpgrades = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
            if (!animator.GetBool("isDashing"))
                animator.SetBool("isFalling", true);
            else
            animator.SetBool("isJumping", false);
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowFall - 1) * Time.deltaTime;
            animator.SetBool("isFalling", false);
        } else
            animator.SetBool("isFalling", false);

        if (animator.GetBool("isWallGrabbed"))
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("Moving", false);
        }

        IsOnGround();
        animator.SetBool("isgrounded", canJump);
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump)
                canDoubleJump = true;

            if (canJump && !IsOnWall())
                Jump(1f);
            if (canDoubleJump && !canJump && !IsOnWall())
            {
                if(animator.GetBool("isFalling"))
                    Jump(0.6f);
                else
                    Jump(1f);
                animator.SetTrigger("DoubleJump");
                canDoubleJump=false;
            } 
        }

        if (IsOnWall() && (wallOnLeft ? moveDirection.x > 0 : moveDirection.x < 0) && Input.GetButton("Jump") && canWallJump && !canJump)
        {
            Jump(0.75f);
            canDoubleJump = false;
            canWallJump = false;
        }

        if (IsOnWall() && (rb.velocity.y >= -5f && rb.velocity.y <= 5f) && !canJump)
        {
            canWallJump = true;
            animator.SetBool("isWallGrabbed", true);
        }
        else
            animator.SetBool("isWallGrabbed", false);


        if (dashCooldownCount > 0)
            dashCooldownCount -= Time.deltaTime;

        if(Input.GetButtonDown("Dash") && dashCooldownCount <= 0 && moveDirection.x != 0)
        {
            dashCooldownCount = dashCooldown;
            invincible = true;
            animator.SetBool("isDashing", true);
        }

        if (Input.GetAxis("Vertical") < 0 && oneWayPlatform != null)
            StartCoroutine(FallThrough());
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

        if (animator.GetBool("isDashing"))
        {
            StartCoroutine(Dash());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
            oneWayPlatform = collision.gameObject;
        if (collision.gameObject.layer == enemyLayer)
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), collision.collider);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("OneWayPlatform"))
            oneWayPlatform = null;
        if (collision.gameObject.layer == enemyLayer)
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), collision.collider, false);
    }

    public IEnumerator FallThrough()
    {
        TilemapCollider2D platformCollider = oneWayPlatform.GetComponent<TilemapCollider2D>();
        CapsuleCollider2D playerCollider = GetComponent<CapsuleCollider2D>();

        Physics2D.IgnoreCollision(platformCollider, playerCollider);

        yield return new WaitForSeconds(0.5f);
        if(!playerCollider.IsTouching(platformCollider))
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
    }

    public void Jump(float divider)
    {
        if (divider == 1f)
            animator.SetBool("isJumping", true);
        else
            animator.SetBool("isWallGrabbed", false);
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpSpeed/divider);
    }

    public IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        rb.AddForce(new Vector2(dashSpeed * moveDirection.x * Time.deltaTime, 0), ForceMode2D.Impulse);
        rb.velocity = new Vector2(rb.velocity.x, 0);

        yield return new WaitForSeconds(0.15f);

        animator.SetBool("isDashing", false);
        Physics2D.IgnoreLayerCollision(6, 7, false);
        invincible = false;
    }

    public void IsOnGround()
    {
        canJump = Physics2D.BoxCast(GetComponent<CapsuleCollider2D>().bounds.center, GetComponent<CapsuleCollider2D>().bounds.size, 0f, Vector2.down, 0.25f, groundLayer);
    }

    public bool IsOnWall()
    {
        RaycastHit2D wall;
        if (!canJump)
        {
            if (Physics2D.BoxCast(GetComponent<CapsuleCollider2D>().bounds.center, GetComponent<CapsuleCollider2D>().bounds.size, 0f, Vector2.right, 0.15f, groundLayer))
            {
                wall = Physics2D.BoxCast(GetComponent<CapsuleCollider2D>().bounds.center, GetComponent<CapsuleCollider2D>().bounds.size, 0f, Vector2.right, 0.15f, groundLayer);
                if (wall.collider.gameObject.CompareTag("OneWayPlatform"))
                    return false;
                else
                {
                    wallOnLeft = false;
                    return true;
                }
            }
            else if (Physics2D.BoxCast(GetComponent<CapsuleCollider2D>().bounds.center, GetComponent<CapsuleCollider2D>().bounds.size, 0f, Vector2.left, 0.15f, groundLayer))
            {
                wall = Physics2D.BoxCast(GetComponent<CapsuleCollider2D>().bounds.center, GetComponent<CapsuleCollider2D>().bounds.size, 0f, Vector2.left, 0.15f, groundLayer);
                if (wall.collider.gameObject.gameObject.CompareTag("OneWayPlatform"))
                    return false;
                else
                {
                    wallOnLeft = true;
                    return true;
                }
            }
        }
        return false;
    }
}
