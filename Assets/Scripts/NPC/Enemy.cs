using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float angle;
    public float radius;
    public float height;
    public float delay;
    public float maxHealth;
    [SerializeField] protected float health;

    protected GameObject player;
    protected Vector2 direction;
    protected bool canSeePlayer;

    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask obstruction;
    // Start is called before the first frame update
    protected void Exist()
    {
        player = GameObject.FindWithTag("Player");
        health = maxHealth;
        StartCoroutine(FieldOfView(delay));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(gameObject.tag))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), collision.collider);
        }
    }

    public IEnumerator FieldOfView(float delay)
    {
        WaitForSeconds delaySeconds = new WaitForSeconds(delay);

        while(true)
        {
            yield return delaySeconds;
            if (!player.activeSelf)
                player = GameObject.FindWithTag("Player");
            CheckPosition();
        }
    }
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawLine(new Vector2(transform.position.x+2, transform.position.y), new Vector2(transform.position.x + 2, transform.position.y + height));
        Gizmos.DrawLine(new Vector2(transform.position.x + 2, transform.position.y), new Vector2(transform.position.x + 2, transform.position.y - height));
    }

    private void CheckPosition()
    {
        Collider2D rangeObj = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if(rangeObj != null && Mathf.Abs(rangeObj.transform.position.y - transform.position.y) <= height)
        {
            direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized;
            if(Vector2.Angle(transform.position, direction) < angle/2)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);

                if (Physics2D.Raycast(transform.position, direction, distance, obstruction))
                    canSeePlayer = false;
                else
                    canSeePlayer = true;
            }
            else
                canSeePlayer = false;
        } else
            canSeePlayer = false;
    }

    public virtual void Hit(float damage, Vector2 d)
    {
        health -= damage;
    }
}
