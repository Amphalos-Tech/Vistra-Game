using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ContinueButtonBreakPoint : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data) => SceneManager.LoadScene($"{SaveHandler.Stage}Dialogue");
}
