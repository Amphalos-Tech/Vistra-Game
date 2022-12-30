using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VistraFileSystem;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

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
    public void OnPointerClick(PointerEventData data) => SceneManager.LoadScene($"{SaveHandler.Stage}Dialogue");
}
