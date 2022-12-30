using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResumeButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject pauseMenu;
    public void OnPointerClick(PointerEventData data)
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
