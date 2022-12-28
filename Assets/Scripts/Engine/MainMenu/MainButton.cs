using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VistraFileSystem;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        if (SaveHandler.Stage != SaveFile.GameStage.Tutorial)
        {
            GetComponent<Text>().text = "Continue";
        }
    }
    public void OnPointerClick(PointerEventData data)
    {
        switch(SaveHandler.Stage)
        {
            case SaveFile.GameStage.Tutorial:
                SceneManager.LoadScene("Tutorial");
                break;
            case SaveFile.GameStage.NeuvistranMeet:
                SceneManager.LoadScene("NeuvistranMeet");
                break;
            case SaveFile.GameStage.NaPaMeet:
                SceneManager.LoadScene("NaPaMeet");
                break;
            case SaveFile.GameStage.Infiltration:
                SceneManager.LoadScene("Infiltration");
                break;
            case SaveFile.GameStage.NeuvistranGeneral:
                SceneManager.LoadScene("NeuvistranGeneral");
                break;
            case SaveFile.GameStage.Castle:
                SceneManager.LoadScene("Castle");
                break;
            case SaveFile.GameStage.NaPaGeneral:
                SceneManager.LoadScene("NaPaGeneral");
                break;
            case SaveFile.GameStage.Epilogue:
                SceneManager.LoadScene("Epilogue");
                break;

            default:
                Debug.Log("An error occurred in loading the correct scene");
                break;
        }
    }
}
