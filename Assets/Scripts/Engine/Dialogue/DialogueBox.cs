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
        textbox.fontSize = CorrespondingFontSize(Settings.Instance.Size);
        delay = CorrespondingDelay(Settings.Instance.Speed);
        textbox = GetComponentInChildren<Text>();
        DialogueHandler.LoadLines();
    }

    void AnimateText()
    {
        if (_text != null)
        {
            StartCoroutine(Animate());
            animating = true;
        }
        else
            SceneManager.LoadScene(SaveHandler.Stage.ToString());
    }

    IEnumerator Animate()
    {
        for (int i = 0; i < _text.Length; i++)
        {
            textbox.text += _text[i];
            yield return new WaitForSeconds(delay);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (animating)
            {
                StopAllCoroutines();
                textbox.text = _text;
                animating = false;
            }
            else
            {
                textbox.text = "";
                var block = DialogueHandler.GetNextDialogueBlock();
                Text = block.Words;
                speakerBox.Speaker = block.Speaker;
            }
        }
    }

    int CorrespondingFontSize(Settings.TextSize fontSize)
    {
        switch (fontSize)
        {
            case Settings.TextSize.Small:
                return 22;
            case Settings.TextSize.Medium:
                return 27;
            case Settings.TextSize.Large:
                return 32;
            default:
                Debug.Log("An error occured loading text size into dialogue");
                return 32;
        }
    }

    float CorrespondingDelay(Settings.TextSpeed speed)
    {
        switch (speed)
        {
            case Settings.TextSpeed.Instant:
                return 0;
            case Settings.TextSpeed.Fast:
                return Time.deltaTime;
            case Settings.TextSpeed.Medium:
                return Time.deltaTime * 3;
            case Settings.TextSpeed.Slow:
                return Time.deltaTime * 5;
            default:
                Debug.Log("An error occured loading text speed into dialogue");
                return Time.deltaTime * 3;
        }
    }
}
