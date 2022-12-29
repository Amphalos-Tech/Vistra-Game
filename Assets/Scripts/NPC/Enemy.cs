using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float angle;
    public float radius;
    public float delay;

    protected GameObject player;
    protected Vector2 direction;
    protected bool canSeePlayer;

    public LayerMask playerLayer;
    public LayerMask obstruction;

    private GameObject[] players;
    // Start is called before the first frame update
    protected void Exist()
    {
        player = GameObject.FindWithTag("Player");
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void CheckPosition()
    {
        Collider2D rangeObj = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        if(rangeObj != null)
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
}
