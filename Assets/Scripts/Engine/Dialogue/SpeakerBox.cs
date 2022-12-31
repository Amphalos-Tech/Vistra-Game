using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerBox : MonoBehaviour
{
    private Image speakerImage;
    private Image speakerFrame;

    [SerializeField]
    private GameObject speakerObject;

    private Sprite[] images;

    private Sprite[] frames;

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
    void Awake()
    {
        speakerFrame = GetComponent<Image>();
        speakerImage = transform.GetChild(0).GetComponent<Image>();
        speakerNametag = speakerObject.GetComponent<Text>();

        images = Resources.LoadAll<Sprite>("Portraits");
        frames = Resources.LoadAll<Sprite>("ImageFrames");
    }

    void SetSpeaker() //Warning: Hard coded values present. Remember to update accordingly.
    {
        var speakerElements = _speaker.Split('_');
        speakerNametag.text = speakerElements[0];

        if (speakerElements[0].Equals("MC1") || speakerElements[0].Equals("MC2"))
        {
            Debug.Log("reached line 49");
            speakerFrame.sprite = frames[1];
        }
        else speakerFrame.sprite = frames[0]; //MCs get portal frame, Vistrans get ice frame

        speakerImage.sprite = images.Where(img => img.name.Equals(_speaker)).ElementAt(0); //set matching portrait
        Debug.Log($"Speaker Frame Sprite Should be vs is: {frames[1].name}, {speakerFrame.sprite}");
    }
}
