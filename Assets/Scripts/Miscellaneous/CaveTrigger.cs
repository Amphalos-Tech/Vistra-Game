using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveTrigger : MonoBehaviour
{
    public bool left;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player != null)
            {
                Sight(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                Sight(player);
            }
        }
    }

    private void Sight(Player player)
    {
        if(left)
        {
            if (player.rb.velocity.x < 0)
                player.ReduceSight(false);
            else
            {
                player.reducedSight = true;
                player.ReduceSight(true);
            }
        } else
        {
            if (player.rb.velocity.x > 0)
                player.ReduceSight(false);
            else
            {
                player.reducedSight = true;
                player.ReduceSight(true);
            }
        }
    }
}
