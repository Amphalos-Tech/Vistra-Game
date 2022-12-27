using TMPro;
using UnityEngine;

public class QualityDropdown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TMP_Dropdown>().onValueChanged.AddListener(
            (val) => Settings.Instance.Quality = (Settings.QualityType)val
            );
        GetComponentInChildren<TMP_Dropdown>().value = (int)Settings.Instance.Quality;
    }
}
