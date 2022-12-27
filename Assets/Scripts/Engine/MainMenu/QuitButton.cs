using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour, IPointerClickHandler
{
    Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("Quit Clicked");
        text.CrossFadeAlpha(.5f, .1f, false);
        Application.Quit();
    }
}
