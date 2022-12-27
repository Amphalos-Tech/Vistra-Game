using SaveSystem;
using System.IO;
using UnityEngine;

public static class SaveHandler
{
    private static SaveFile save;

    public static byte[] Volumes => save.Settings[0..3];

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

    public static void CreateSave() //TODO
    {
        SaveSystem.SaveSystem.Write(save);
    }
}
