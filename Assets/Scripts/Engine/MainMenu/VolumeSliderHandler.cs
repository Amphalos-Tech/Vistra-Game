using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() 
    {
        Slider[] sliders = gameObject.GetComponentsInChildren<Slider>();
        //This was the only way because assigning a refernece to a local variable would use that variable, not the value it was assigned when set
        sliders[0].onValueChanged.AddListener((val) => Settings.Instance.Volumes[0] = (byte)val);
        sliders[1].onValueChanged.AddListener((val) => Settings.Instance.Volumes[1] = (byte)val);
        sliders[2].onValueChanged.AddListener((val) => Settings.Instance.Volumes[2] = (byte)val);
        sliders[3].onValueChanged.AddListener((val) => Settings.Instance.Volumes[3] = (byte)val);

        sliders[0].value = Settings.Instance.Volumes[0];
        sliders[1].value = Settings.Instance.Volumes[1];
        sliders[2].value = Settings.Instance.Volumes[2];
        sliders[3].value = Settings.Instance.Volumes[3];
    }
}
