using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerSpawn : MonoBehaviour
{
    public bool vertical;
    public bool downspawn;
    public bool rightspawn;
    public bool childcollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (!childcollider)
            {
                if (vertical)
                {
                    if (downspawn)
                    {
                        gameObject.transform.GetChild(0).gameObject.SetActive(rb.velocity.y < 0);
                    }
                    else
                    {
                        gameObject.transform.GetChild(0).gameObject.SetActive(rb.velocity.y > 0);
                    }
                }
                else
                {
                    if (rightspawn)
                        gameObject.transform.GetChild(0).gameObject.SetActive(rb.velocity.x > 0);
                    else
                        gameObject.transform.GetChild(0).gameObject.SetActive(rb.velocity.x < 0);
                }
            }
            else
            {
                if (vertical)
                {
                    if (downspawn)
                    {
                        gameObject.transform.parent.GetChild(0).gameObject.SetActive(rb.velocity.y < 0);
                    }
                    else
                    {
                        gameObject.transform.parent.GetChild(0).gameObject.SetActive(rb.velocity.y > 0);
                    }
                }
                else
                {
                    if (rightspawn)
                        gameObject.transform.parent.GetChild(0).gameObject.SetActive(rb.velocity.x > 0);
                    else
                        gameObject.transform.parent.GetChild(0).gameObject.SetActive(rb.velocity.x < 0);
                }

            }

        }

    }
}
