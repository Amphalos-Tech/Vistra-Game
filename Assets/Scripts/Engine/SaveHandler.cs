using SaveSystem;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveHandler
{
    private static SaveFile save;

    public static byte[] Volumes => save.Settings[0..4]; //0 inclusive to 4 exclusive

    public static SaveFile.GameStage Stage => save.Stage;

    public static Settings.QualityType Quality => (Settings.QualityType)save.Settings[4];

    public static byte[] Upgrades => save.Upgrades; //Potential TODO
    static SaveHandler()
    {
        var path = Application.dataPath + "/";
        SaveSystem.SaveSystem.Path = path;
        if (!File.Exists(path + "encrypted.xml"))
            SaveSystem.SaveSystem.Write(new SaveFile());
    }
    public static void LoadSave() 
    {
        save = SaveSystem.SaveSystem.Read();
    }

    public static void CreateSave(SaveFile.GameStage stage) //stage represents stage reached, aka stage to start on upon continue
    {
        var s = new SaveFile();
        var settings = new byte[5];
        Settings.Instance.Volumes.CopyTo(settings, 0);
        settings[4] = (byte)Settings.Instance.Quality;
        s.Settings = settings;
        //UNCOMMENT ONCE MERGED: s.Upgrades = Player.upgrades;
        s.Stage = stage;
        SaveSystem.SaveSystem.Write(s);
    }
}
