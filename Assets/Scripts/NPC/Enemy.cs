using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float angle;
    public float radius;
    public float delay;

    protected Vector2 playerpos;
    protected Player playerClass;
    protected bool canSeePlayer;

    public LayerMask playerLayer;
    public LayerMask obstruction;
    // Start is called before the first frame update
    void Start()
    {
        playerpos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        playerClass = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(FieldOfView(delay));
    }

    public IEnumerator FieldOfView(float delay)
    {
        WaitForSeconds delaySeconds = new WaitForSeconds(delay);

        while(true)
        {
            yield return delaySeconds;
            CheckPosition();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void CheckPosition()
    {
        Collider2D rangeObj = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if(rangeObj != null)
        {
            Vector2 direction = new Vector2(playerpos.x - transform.position.x, playerpos.y - transform.position.y).normalized;
            if(Vector2.Angle(transform.position, direction) < angle/2)
            {
                float distance = Vector2.Distance(transform.position, playerpos);

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
}
