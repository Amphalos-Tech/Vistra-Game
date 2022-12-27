using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject main;
    [SerializeField]
    GameObject settings;

    public void OnPointerClick(PointerEventData data)
    {
        main.SetActive(true);
        settings.SetActive(false);
    }
}
