using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerSpawn : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
