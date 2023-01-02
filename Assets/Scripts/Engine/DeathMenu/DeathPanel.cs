using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanel : MonoBehaviour
{
    private GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        panel = transform.GetChild(0).gameObject;
        StartCoroutine("DetectDeath");
    }

    IEnumerator DetectDeath()
    {
        while (true)
        {
            if (PausePanel.dead)
            {
                yield return new WaitForSeconds(.5f);
                Cursor.lockState = CursorLockMode.None;
                panel.SetActive(true);
                yield break;
            }

            yield return new WaitForSeconds(.5f);
        }
    }
}
