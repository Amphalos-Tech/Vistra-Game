using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    Text textbox;

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            AnimateText();

        }
    }

    private string _text;

    // Start is called before the first frame update
    void Start()
    {
        textbox = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AnimateText()
    {

    }
}
