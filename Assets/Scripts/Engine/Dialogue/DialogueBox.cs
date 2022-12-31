using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using VistraFileSystem;

public class DialogueBox : MonoBehaviour //ADD A SKIP DIALOGUE LATER
{
    [SerializeField]
    private SpeakerBox speakerBox;

    Text textbox;
    bool animating = false;
    float delay;

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
        textbox.fontSize = Settings.CorrespondingFontSize();
        delay = Settings.CorrespondingDelay();
        DialogueHandler.LoadLines();
        DisplayNextBlock();
    }

    void AnimateText()
    {
        StartCoroutine(Animate());
        animating = true;
    }

    IEnumerator Animate()
    {
        for (int i = 0; i < _text.Length; i++)
        {
            textbox.text += _text[i];
            yield return new WaitForSeconds(delay);
        }
        animating = false;
    }

    void Update()
    {
        if (Input.anyKeyDown && Time.timeScale != 0 && !Input.GetKey(KeyCode.Escape)) //Any key while not paused besides pause
        {
            if (animating)
            {
                StopAllCoroutines();
                textbox.text = _text;
                animating = false;
            }
            else
            {
               DisplayNextBlock();
            }
        }
    }

    void DisplayNextBlock()
    {
        var block = DialogueHandler.GetNextDialogueBlock();
        if (block != null)
        {
            textbox.text = "";
            Text = block.Words;
            speakerBox.Speaker = block.Speaker;
        }
        else SceneManager.LoadScene(SaveHandler.Stage.ToString());
    }
}
