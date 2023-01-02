using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ReturnToTitleBreakPoint : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        SaveHandler.LoadSave();
    }

    public void OnPointerClick(PointerEventData data)
    {
        SceneManager.LoadScene("Main Menu");
    }
}
