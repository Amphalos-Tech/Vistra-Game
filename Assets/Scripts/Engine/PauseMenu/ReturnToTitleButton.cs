using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ReturnToTitleButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject resumeButton;

    [SerializeField]
    GameObject panel;

    public void OnPointerClick(PointerEventData data)
    {
        resumeButton.SetActive(false);
        panel.SetActive(true);
        gameObject.SetActive(false);
    }
}
