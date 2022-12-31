using UnityEngine;

public class Settings : MonoBehaviour
{
    //Singleton
    public static Settings Instance { get; set; } = null;

    //Declarations
    public enum QualityType
    {
        Max = 0,
        High = 1,
        Medium = 2,
        Low = 3
    }

    public enum TextSpeed
    {
        Hyper = 0,
        Fast = 1,
        Medium = 2,
        Slow = 3
    }

    public enum TextSize
    {
        Small = 0,
        Medium = 1, 
        Large = 2
    }

    //Properties
    public byte[] Volumes { get; set; } = { 100, 100, 100, 100 }; 
    public QualityType Quality { get; set; } = QualityType.Max;
    public TextSpeed Speed { get; set; } = TextSpeed.Fast;
    public TextSize Size { get; set; } = TextSize.Medium;

    private void Awake()
    {

        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            SaveHandler.LoadSave();
            Volumes = SaveHandler.Volumes;
            Quality = SaveHandler.Quality;
            Size = SaveHandler.Size;
            Speed = SaveHandler.Speed;
            Instance = this;
        }
    }
    public static float CorrespondingDelay() //Following two can be changed to modify whole project
    {
        switch (Instance.Speed)
        {
            case TextSpeed.Hyper:
                return 0;
            case TextSpeed.Fast:
                return Time.deltaTime;
            case TextSpeed.Medium:
                return Time.deltaTime * 3;
            case TextSpeed.Slow:
                return Time.deltaTime * 5;
            default:
                Debug.Log("An error occured loading text speed into text");
                return Time.deltaTime * 3;
        }
    }

    public static int CorrespondingFontSize()
    {
        switch (Instance.Size)
        {
            case TextSize.Small:
                return 22;
            case TextSize.Medium:
                return 27;
            case TextSize.Large:
                return 32;
            default:
                Debug.Log("An error occured loading text size into text");
                return 32;
        }
    }
}
