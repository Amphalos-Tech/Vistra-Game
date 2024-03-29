using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private GameObject panel;
    public static bool dead;

    void Awake() => dead = false;

    // Start is called before the first frame update
    void Start()
    {
        panel = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !dead)
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1; //Swaps between normal and frozen
            panel.SetActive(!panel.activeInHierarchy); //Swaps between active and inactive
            ToggleCursorLockIfNecessary();
        }
    }

    public static void ToggleCursorLockIfNecessary()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (player.GetComponent<Player>().meleeMC) //Toggles cursor lock and unlock if melee
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
}
