using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public float maxHealth;
    public static float health;
    public int maxAmmo;
    public static int ammo;

    [SerializeField]
    private HealthAmmoPanel uiPanel;

    public StateMachine machine;

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
    public GameObject rangedBullet;

    private float dashCooldownCount;

    public Animator animator;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    public bool canJump;
    private bool canDoubleJump;
    private bool wallOnLeft;
    private bool canWallJump;
    private bool canDash;
    public bool hit;
    private GameObject otherPlayer;
    public static byte[] upgrades;
    private static bool loadedUpgrades = false;

    private bool attack1;
    private bool attack2;
    private bool attack3;
    private bool attack4;
    private float rotz;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        uiPanel.SetMaxHealth(maxHealth);
        ammo = maxAmmo;
        uiPanel.SetMaxAmmo(maxAmmo);
        if (meleeMC)
            machine = GetComponent<StateMachine>();
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
        if (Time.timeScale != 0)
            UpdateAsUsual();
    }

    void UpdateAsUsual()
    {
        if (!meleeMC)
        {
            Vector2 mousedir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rotz = Mathf.Atan2(mousedir.y, mousedir.x) * Mathf.Rad2Deg;
        }

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

        animator.SetBool("isgrounded", canJump);
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
        if (Input.GetButtonDown("Jump") && !hit)
        {
            if (canJump)
                canDoubleJump = true;

            if (canJump && !IsOnWall())
                Jump(1f);
            if (canDoubleJump && !canJump && !animator.GetBool("isWallGrabbed"))
            {
                if (animator.GetBool("isFalling"))
                    Jump(0.6f);
                else
                    Jump(1f);
                animator.SetTrigger("DoubleJump");
                canDoubleJump = false;
            }
        }

        if (IsOnWall() && (wallOnLeft ? moveDirection.x > 0 : moveDirection.x < 0) && Input.GetButton("Jump") && canWallJump && !canJump)
        {
            animator.SetTrigger("Walljump");
            animator.SetBool("isWallGrabbed", false);
            Jump(0.75f);
            canDoubleJump = false;
            canWallJump = false;
        }

        if (IsOnWall() && (rb.velocity.y >= -5f && rb.velocity.y <= 5f) && !canJump && (wallOnLeft ? moveDirection.x < 0 : moveDirection.x > 0))
        {
            canWallJump = true;
            animator.SetBool("isWallGrabbed", true);
        }
        else
            animator.SetBool("isWallGrabbed", false);


        if (dashCooldownCount > 0)
            dashCooldownCount -= Time.deltaTime;

        if (Input.GetButtonDown("Dash") && dashCooldownCount <= 0 && moveDirection.x != 0 && !hit)
        {
            dashCooldownCount = dashCooldown;
            invincible = true;
            animator.SetBool("isDashing", true);
        }

        if (Input.GetButtonDown("Swap") && !animator.GetBool("isDashing"))
            Swap();

        if (Input.GetButtonDown("Attack") && meleeMC && machine.CurrentState.GetType() == typeof(Idle) && !animator.GetBool("isDashing"))
            machine.SetNextState(new MeleeEntryState());
        else if (Input.GetButtonDown("Attack") && !meleeMC && !animator.GetBool("isDashing") && !animator.GetBool("isWallGrabbed") && ammo-- > 0)
        {
            if (canJump)
            {
                rb.velocity = Vector2.zero;
                hit = true;
            }
            Ranged();
        }

        if (attack1)
        {
            Attack1(2);
        }
        else if (attack2)
        {
            Attack2(2);
        }
        else if (attack3)
        {
            Attack3(6);
        }
        else if (attack4)
        {
            Attack4(8);
        }

        if (health <= 0)
        {
            rb.velocity = Vector2.zero;
            gameObject.tag = "Untagged";
            gameObject.layer = 0;
            PausePanel.dead = true;
            animator.SetTrigger("Die");
            GetComponent<Player>().enabled = false;
        }
    }

    public void Reset()
    {
        hit = false;
    }

    void FixedUpdate()
    {
        if (!hit)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);

            if (rb.velocity.x < 0)
            {
                if (canJump)
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
        }

        if (animator.GetBool("isDashing"))
        {
            canDash = true;
            StartCoroutine(Dash());
        }

        if (transform.position.y <= 0)
        {
            health = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Jump(float divider)
    {
        animator.SetBool("isFalling", false);
        if (divider == 1f)
            animator.SetBool("isJumping", true);
        else if (divider == 0.75f)
        {
            animator.SetBool("isWallGrabbed", false);
        }
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpSpeed/divider);
    }

    public IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        if (canDash)
        {
            invincible = true;
            rb.AddForce(new Vector2(dashSpeed * moveDirection.x * Time.deltaTime, 0), ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        else
            rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.15f);

        animator.SetBool("isDashing", false);
        Physics2D.IgnoreLayerCollision(6, 7, false);
        invincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            if (animator.GetBool("isDashing"))
                canDash = false;
        }
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

    public void Hit(float damage, float iframes, Vector2 direction, float knockback, Vector2 enemypos)
    {
        if(!invincible)
        {
            health -= damage;
            uiPanel.Health -= damage;
            StartCoroutine(Shake(0.25f, damage / 25));
            if (meleeMC)
                machine.SetNextState(new Idle());
            else
                Reset();
            StartCoroutine(Stopper(Mathf.Clamp(iframes, 0, 0.5f)));
            if(Mathf.Abs(transform.position.x - enemypos.x) < 2f)
                rb.velocity = new Vector2( direction.x * knockback * 3, rb.velocity.y + jumpSpeed / 2);
            else
                rb.velocity = new Vector2( direction.x * knockback, rb.velocity.y + jumpSpeed/2);
            StartCoroutine(Invincibility(iframes));
        }
    }

    public void Attack1(float damage)
    {
        float x = -3f;
        Vector2 size = new Vector2(9, 4);
        Vector2 direction;
        if (transform.rotation == Quaternion.Euler(0, 0, 0))
            direction = Vector2.right;
        else
        {
            x *= -1;
            direction = Vector2.left;
        }


        RaycastHit2D enemy = Physics2D.BoxCast(new Vector2(transform.position.x + x, transform.position.y), size, 0f, direction, 2.25f, enemyLayer);
        if (enemy)
        {
            attack1 = false;
            GameObject hitEnemy = enemy.collider.gameObject;
            if (hitEnemy != null)
            {
                Vector2 dir = new Vector2(hitEnemy.transform.position.x - transform.position.x, hitEnemy.transform.position.y - transform.position.y).normalized;
                hitEnemy.GetComponent<Enemy>().Hit(damage, dir);    
                if (ammo < maxAmmo)
                    uiPanel.Ammo = ++ammo;
            }
        }
    }

    public void Attack2(float damage)
    {
        float x = -3f;
        Vector2 size = new Vector2(9, 3);
        Vector2 direction;
        if (transform.rotation == Quaternion.Euler(0, 0, 0))
            direction = Vector2.right;
        else
        {
            direction = Vector2.left;
            x *= -1;
        }

        RaycastHit2D enemy = Physics2D.BoxCast(new Vector2(transform.position.x + x, transform.position.y), size, 0f, direction, 2f, enemyLayer);
        if (enemy)
        {
            attack2 = false;
            GameObject hitEnemy = enemy.collider.gameObject;
            if (hitEnemy != null)
            {
                Vector2 dir = new Vector2(hitEnemy.transform.position.x - transform.position.x, hitEnemy.transform.position.y - transform.position.y).normalized;
                hitEnemy.GetComponent<Enemy>().Hit(damage, dir);
                if (ammo < maxAmmo)
                    uiPanel.Ammo = ++ammo;
            }
        }
    }

    public void Attack3(float damage)
    {
        float x = -5f;
        Vector2 size = new Vector2(12, 3);
        Vector2 direction;
        if (transform.rotation == Quaternion.Euler(0, 0, 0))
            direction = Vector2.right;
        else
        {
            x *= -1;
            direction = Vector2.left;
        }

        RaycastHit2D enemy = Physics2D.BoxCast(new Vector2(transform.position.x + x, transform.position.y), size, 0f, direction, 4f, enemyLayer);
        if (enemy)
        {
            attack3 = false;
            GameObject hitEnemy = enemy.collider.gameObject;
            if (hitEnemy != null)
            {
                Vector2 dir = new Vector2(hitEnemy.transform.position.x - transform.position.x, hitEnemy.transform.position.y - transform.position.y).normalized;
                hitEnemy.GetComponent<Enemy>().Hit(damage, dir);
                if (ammo < maxAmmo)
                    uiPanel.Ammo = ++ammo;
            }
        }
    }

    public void Attack4(float damage)
    {
        float x = -5f;
        Vector2 size = new Vector2(12, 8);
        Vector2 direction;
        if (transform.rotation == Quaternion.Euler(0, 0, 0))
            direction = Vector2.right;
        else
        {
            x *= -1;
            direction = Vector2.left;
        }

        RaycastHit2D enemy = Physics2D.BoxCast(new Vector2(transform.position.x + x, transform.position.y), size, 0f, direction, 3.5f, enemyLayer);
        if (enemy)
        {
            attack4 = false;
            GameObject hitEnemy = enemy.collider.gameObject;
            if (hitEnemy != null)
            {
                Vector2 dir = new Vector2(hitEnemy.transform.position.x - transform.position.x, hitEnemy.transform.position.y - transform.position.y).normalized;
                StartCoroutine(Shake(0.1f, 0.5f));
                hitEnemy.GetComponent<Enemy>().Hit(damage, dir);
                if (ammo < maxAmmo)
                    uiPanel.Ammo = ++ammo;
            }
        }
    }


    public void Enable1()
    {
        attack1 = true;
    }

    public void Disable1()
    {
        attack1 = false;
    }
    public void Enable2()
    {
        attack2 = true;
    }

    public void Disable2()
    {
        attack2 = false;
    }

    public void Enable3()
    {
        attack3 = true;
    }

    public void Disable3()
    {
        attack3 = false;
    }
    public void Enable4()
    {
        attack4 = true;
    }

    public void Disable4()
    {
        attack4 = false;
    }

    public IEnumerator Stopper(float time)
    {
        hit = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(time);
        hit = false;
    }

    IEnumerator Invincibility(float iframes)
    {
        invincible = true;
        yield return new WaitForSeconds(iframes);
        invincible = false;
    }

    public void Ranged()
    {
        float x = 0f;
        float y = 0f;

        if (transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            if(rotz >-90 && rotz <= 100)
            {
                hit = false;
                x = 0;
                y = 0;
                ammo++;
            } else if ((rotz <= -170 || rotz > 170f)) {
                animator.SetTrigger("Shooting");
                x = 3.125f;
                y = 1.23f;
            }
            else if ((rotz < 170 && rotz > 0)) {
                animator.SetTrigger("Shooting Down");
                x = 3.07f;
                y = -0.3f;
            }
            else if ((rotz > -170 && rotz <= -130)) {
                animator.SetTrigger("Shooting Up");
                x = 3.07f;
                y = 2.1f;
            }
            else if ((rotz <= -90 && rotz > -130)) {
                animator.SetTrigger("Shooting Straight Up");
                x = 1.14f;
                y = 3.4f;
            }

            if (rotz < 180 && rotz > 0)
                rotz -= 360;
            rotz = Mathf.Clamp(rotz, -210f, -90f);
        }
        else
        {
            if((rotz < -90 && rotz > -180) || (rotz > 80 && rotz <= 180))
            {
                hit = false;
                x = 0;
                y = 0;
                ammo++;
            } else if(((rotz >= -10f && rotz <= 0) || (rotz <= 10f && rotz > 0)))
            {
                animator.SetTrigger("Shooting");
                x = -3.125f;
                y = 0.48f;
            } else if((rotz < 170 && rotz > 0) || (rotz < 10 && rotz > 0))
            {
                animator.SetTrigger("Shooting Down");
                x = -3.07f;
                y = -1.28f;
            } else if((rotz < -10 && rotz >= -70))
            {
                animator.SetTrigger("Shooting Up");
                x = -3.07f;
                y = 1.35f;
            } else if((rotz > -90 && rotz < -70))
            {
                animator.SetTrigger("Shooting Straight Up");
                x = -2.14f;
                y = 3.4f;
            }

            if (rotz > 30)
                rotz = 30;
            else if (rotz < -90)
                rotz = -90;
        }


        

        GameObject bullet = Instantiate(rangedBullet, new Vector2(transform.position.x + x, transform.position.y + y), Quaternion.Euler(0, 0, rotz));
        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * -0.01f, ForceMode2D.Impulse);

        if (uiPanel.Ammo != ammo)
            uiPanel.Ammo = ammo;
    }

    public IEnumerator Shake(float time, float amount)
    {
        //Vector3 ogpos = Camera.main.transform.position;
        float timeElapsed = 0f;

        while (timeElapsed < time)
        {
            if (Time.timeScale > 0)
            {
                float x = UnityEngine.Random.Range(-1f, 1f) * amount;
                float z = UnityEngine.Random.Range(-1f, 1f) * amount;
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + x, Camera.main.transform.position.y, Camera.main.transform.position.z + z);
                timeElapsed += Time.deltaTime;
            }
            yield return 0;
        }

        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y + Camera.main.GetComponent<CameraFollow>().yoffset, -10);
    }
}
