using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    private GameObject player;
    private float xDistanceToLoad = 10f, yDistanceToLoad = 1f, delay;
    private TMP_Text text;
    private string initialText;
    // Start is called before the first frame update
    void Start()
    {
        delay = Settings.CorrespondingDelay();
        text = GetComponent<TMP_Text>();
        initialText = text.text;
        text.text = "";
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("DetectPlayer");
    }

    IEnumerable DetectPlayer()
    {
        while (true)
        {
            if (!player.activeSelf)
                player = GameObject.FindGameObjectWithTag("Player");
            var posDif = player.transform.position - gameObject.transform.position;
            if (Mathf.Abs(posDif.x) < xDistanceToLoad && Mathf.Abs(posDif.y) < yDistanceToLoad)
            {
                StartCoroutine("AnimateText");
                yield break;
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerable AnimateText()
    {
        for (byte i = 0; i < initialText.Length; i++)
        {
            text.text += initialText[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
