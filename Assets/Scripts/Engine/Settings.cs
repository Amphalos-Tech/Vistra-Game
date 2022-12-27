using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; set; } = null;
    public enum QualityType
    {
        Max = 0,
        High = 1,
        Medium = 2,
        Low = 3
    }

    public byte[] Volumes { get; set; } = { 100, 100, 100, 100 }; 
    public QualityType Quality { get; set; } = QualityType.Max;
    private void Awake()
    {

        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}
