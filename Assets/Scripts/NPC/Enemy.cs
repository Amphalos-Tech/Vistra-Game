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
    protected Vector2 enemyDirection;
    protected bool canSeePlayer;
    protected bool canSeeEnemy;

    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask obstruction;
    public LayerMask random;

    private GameObject[] players;
    // Start is called before the first frame update
    protected void Exist()
    {
        player = GameObject.FindWithTag("Player");
        health = maxHealth;
        StartCoroutine(FieldOfView(delay));
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
        //gameObject.layer = random;
        Collider2D rangeEnemy = Physics2D.OverlapCircle(transform.position, radius, enemyLayer);
        //gameObject.layer = enemyLayer;
        /*
        if (rangeEnemy != null && rangeEnemy.gameObject.tag == gameObject.tag)
            rangeEnemy = null;
        */
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
        /*
        if (rangeEnemy != null && Mathf.Abs(rangeEnemy.transform.position.y - transform.position.y) <= height)
        {
            enemyDirection = new Vector2(rangeEnemy.gameObject.transform.position.x - transform.position.x, rangeEnemy.gameObject.transform.position.y - transform.position.y).normalized;
            if (Vector2.Angle(transform.position, direction) < angle / 2)
            {
                float distance = Vector2.Distance(transform.position, rangeEnemy.transform.position);

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
        */
    }

    public virtual void Hit(float damage, Vector2 d)
    {
        health -= damage;
    }
}
