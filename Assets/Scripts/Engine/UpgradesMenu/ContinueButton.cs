using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using VistraFileSystem;

public class ContinueButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Dropdown dropdown;

    private int upgradeChoice;

    // Start is called before the first frame update
    void Start()
    {
        dropdown.onValueChanged.AddListener((val) =>
        {
            upgradeChoice = val;
        });
    }

    public void OnPointerClick(PointerEventData data)
    {
        byte[] newUpgrades = new byte[4]; 
        Player.upgrades.CopyTo(newUpgrades, 0);
        newUpgrades[upgradeChoice]++;
        Player.upgrades = newUpgrades;
        SaveHandler.CreateSave((SaveFile.GameStage)((int)SaveHandler.Stage + 1));
        SceneManager.LoadScene("BreakPoint");
    }
}
