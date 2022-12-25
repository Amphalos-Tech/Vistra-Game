using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float maxVelocity;
    public float jumpSpeed;
    public bool canJump;
    public bool canDoubleJump;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private int jump;
    private bool wallOnLeft;
    private bool wallJumpMove;

    // Start is called before the first frame update
    void Start()
    {
        canDoubleJump = true;
        wallJumpMove = true;
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsOnGround();
            if (canJump || IsOnWall())
                canDoubleJump = true;
            if (canDoubleJump || canJump)
                jump = 1;
            if (canDoubleJump && !canJump)
                canDoubleJump = false;
        }
        else
            jump = 0;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime, rb.velocity.y);
        rb.AddForce(new Vector2(0, jump * jumpSpeed * Time.deltaTime), ForceMode2D.Impulse);
    }

    public void IsOnGround()
    {
        canJump = Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
    }

    public bool IsOnWall()
    {
        if(!canJump)
        {
            if(Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.right, 0.1f, groundLayer))
            {
                wallOnLeft = false;
                return true;
            }
            else if(Physics2D.BoxCast(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.left, 0.1f, groundLayer))
            {
                wallOnLeft = true;
                return true;
            }
        }
        return false;
    }

    public void ResetWallJumpMove()
    {
        wallJumpMove = true;
    }
}
