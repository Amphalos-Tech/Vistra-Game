using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject main;
    [SerializeField]
    GameObject settings;
    public void OnPointerClick(PointerEventData data)
    {
        settings.SetActive(true);
        main.SetActive(false);
    }
}
