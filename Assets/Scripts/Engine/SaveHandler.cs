using VistraFileSystem;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveHandler
{
    private static SaveFile save;

    public static byte[] Volumes => save.Settings[0..4]; //0 inclusive to 4 exclusive

    public static SaveFile.GameStage Stage => save.Stage;

    public static Settings.QualityType Quality => (Settings.QualityType)save.Settings[4];

    public static Settings.TextSize Size => (Settings.TextSize)save.Settings[5];

    public static Settings.TextSpeed Speed => (Settings.TextSpeed)save.Settings[6];

    public static byte[] Upgrades => save.Upgrades; //Potential TODO

    static SaveHandler()
    {
        var path = Application.dataPath + "/";
        SaveSystem.Path = path;
        if (!File.Exists(path + "encrypted.xml"))
            SaveSystem.Write(new SaveFile());
    }
    public static void LoadSave() 
    {
        save = SaveSystem.Read();
    }

    public static void CreateSave(SaveFile.GameStage stage) //stage represents stage reached, aka stage to start on upon continue
    {
        var s = new SaveFile();
        var settings = new byte[7];
        Settings.Instance.Volumes.CopyTo(settings, 0);
        settings[4] = (byte)Settings.Instance.Quality;
        settings[5] = (byte)Settings.Instance.Size;
        settings[6] = (byte)Settings.Instance.Speed;
        s.Settings = settings;
        s.Upgrades = Player.upgrades;
        s.Stage = stage;
        SaveSystem.Write(s);
    }
}
