using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public LayerMask platformLayer;
    public bool meleeMC;
    public float dashSpeed;
    public float dashCooldown;
    public GameObject swapParticles;
    public GameObject swapParticles2;

    private float dashCooldownCount;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool canJump;
    private bool canDoubleJump;
    private bool wallOnLeft;
    private bool canWallJump;
    private GameObject otherPlayer;
    public static byte[] upgrades;
    private static bool loadedUpgrades = false;

    // Start is called before the first frame update
    void Start()
    {
        canDoubleJump = true;
        dashCooldownCount = 0;
        Physics.gravity = new Vector3(0, -9.8f, 0);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 8);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        if(!meleeMC)
            gameObject.SetActive(false);


        if(!loadedUpgrades)
        {
            upgrades = SaveHandler.Upgrades;
            loadedUpgrades = true;
        }
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(0.1f);
        if (!meleeMC)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList();
        players.Remove(gameObject);

        otherPlayer = players[0];

        if(meleeMC)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y < 0 && !canJump)
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
        }
        else
        {
            animator.SetBool("isFalling", false);
            if (animator.GetBool("isJumping"))
                animator.SetBool("isJumping", false);
        }

        if (animator.GetBool("isWallGrabbed"))
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("Moving", false);
        }

        //IsOnGround();
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

        if (Input.GetButtonDown("Dash") && dashCooldownCount <= 0 && moveDirection.x != 0)
        {
            dashCooldownCount = dashCooldown;
            invincible = true;
            animator.SetBool("isDashing", true);
        }

        if (Input.GetButtonDown("Swap") && !invincible)
            Swap();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            canJump = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, 8, false);
            canJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            canJump = false;
        else if(collision.gameObject.CompareTag("OneWayPlatform"))
        {
            canJump = false;
            Physics2D.IgnoreLayerCollision(gameObject.layer, 8  );
        }

    }

    public bool IsOnWall()
    {
        RaycastHit2D wall;
        if (!canJump)
        {
            if (Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.right, 0.15f, groundLayer))
            {
                wall = Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.right, 0.15f, groundLayer);
                if (wall.collider.gameObject.CompareTag("OneWayPlatform"))
                    return false;
                else
                {
                    wallOnLeft = false;
                    return true;
                }
            }
            else if (Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.left, 0.15f, groundLayer))
            {
                wall = Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.left, 0.15f, groundLayer);
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

    void Swap()
    {
        otherPlayer.transform.position = transform.position;
        otherPlayer.transform.rotation = transform.rotation;
        Instantiate(swapParticles, otherPlayer.transform.position, new Quaternion(1f, 0, 0, 0f));
        Instantiate(swapParticles2, otherPlayer.transform.position, new Quaternion(1f, 0, 0, 0f));
        otherPlayer.SetActive(true);
        gameObject.SetActive(false);
    }
}
