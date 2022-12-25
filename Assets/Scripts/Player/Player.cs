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

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        canDoubleJump = true;
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime, rb.velocity.y);
    }
}
