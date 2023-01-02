using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesPanel : MonoBehaviour
{
    private GameObject panel;
    private GameObject player;
    private float xDistanceToLoad = 7.5f, yDistanceToLoad = 7.5f;

    [SerializeField]
    Vector3 goalLocation;

    // Start is called before the first frame update
    void Start()
    {
        panel = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("DetectPlayer");
    }

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            if (!player.activeSelf)
                player = GameObject.FindGameObjectWithTag("Player");
            var posDif = player.transform.position - goalLocation;
            if (Mathf.Abs(posDif.x) < xDistanceToLoad && Mathf.Abs(posDif.y) < yDistanceToLoad)
            {
                panel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                yield break;
            }

            yield return new WaitForSeconds(.5f);
        }
    }
}
