using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDropdowns : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var dropdowns = GetComponentsInChildren<TMP_Dropdown>(); //Size, speed
        
        dropdowns[0].onValueChanged.AddListener
        (
            (val) => Settings.Instance.Size = (Settings.TextSize)val
        );
        dropdowns[0].value = (int)Settings.Instance.Size;
        
        dropdowns[1].onValueChanged.AddListener
        (
            (val) => Settings.Instance.Speed = (Settings.TextSpeed)val
        );
        dropdowns[1].value = (int)Settings.Instance.Speed;
    }
}
