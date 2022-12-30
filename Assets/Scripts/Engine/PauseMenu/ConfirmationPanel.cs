using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField]
    GameObject resumeButton, returnToTitleButton;
    private Button yesButton, noButton;
    // Start is called before the first frame update
    void Start()
    {
        var btns = GetComponentsInChildren<Button>();
        noButton = btns[0];
        yesButton = btns[1];

        noButton.onClick.AddListener(() =>
        {
            resumeButton.SetActive(true);
            returnToTitleButton.SetActive(true);
            gameObject.SetActive(false);
        });
        yesButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main Menu");
        });
    }
}
