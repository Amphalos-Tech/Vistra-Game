using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float yoffset;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = player.transform.position;
        transform.position = new Vector3(pos.x, pos.y + yoffset, transform.position.z);

        if(!player.activeSelf)
            player = GameObject.FindGameObjectWithTag("Player");
    }
}
