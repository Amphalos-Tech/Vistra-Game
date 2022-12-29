using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerBox : MonoBehaviour
{
    private Image speakerImage;

    [SerializeField]
    private GameObject speakerObject;

    private Text speakerNametag;

    private string _speaker;
    public string Speaker
    {
        get =>  _speaker;
        set 
        { 
            _speaker = value;
            SetSpeaker();
        }
    }
    void Start()
    {
        speakerImage = GetComponentInChildren<Image>();
        speakerNametag = speakerObject.GetComponentInChildren<Text>();        
    }

    void SetSpeaker()
    {
        speakerNametag.text = _speaker;
        //TODO once assets made: speakerImage = 
    }
}
