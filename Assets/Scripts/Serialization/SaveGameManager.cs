using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveGameManager : MonoBehaviour {

    public static SaveGameManager instance;
    public SOStats stats;
    public SOCameraPPValue quality;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        LoadGame();
    }

    private void OnDisable()
    {
        SaveGame();
    }

    private void Start()
    {
        //DontDestroyOnLoad(this);
    }

    // USARE SOLO SAVEGAME() E LOADGAME()

    // Salva il savedata in modo MANUALE
    public void Save(SerializableSaveData data, string name)
    {
        if (!Directory.Exists(Application.dataPath + "/saves"))
            Directory.CreateDirectory(Application.dataPath + "/saves");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/saves/" + name + ".dat");

        formatter.Serialize(file, data);

        file.Close();
    }

    // Restituisce il savedata da caricare MANUALMENTE
    public SerializableSaveData Load(string name)
    {
        if (!File.Exists(Application.dataPath + "/saves/" + name + ".dat"))
            return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/saves/" + name + ".dat", FileMode.Open);

        SerializableSaveData data = (SerializableSaveData)formatter.Deserialize(file);

        file.Close();

        return data;
    }

    // Carica in modo AUTOMATICO qualità, easy, normal e hard
    public bool LoadGame()
    {
        if (!SaveGameExists())
            return false;
        
        SerializableSaveData data = Load("SaveData");

        stats.easy = data.easy;
        stats.normal = data.normal;
        stats.hard = data.hard;

        quality.value = data.cameraQuality;

        return true;
    }

    // Restituisce il numero di vittorie nella modalità specificata
    public int HowManyWins(string difficulty)
    {
        if (!SaveGameExists())
            return 0;

        SerializableSaveData data = Load("SaveData");

        if (difficulty == "easy")
            return data.easy;
        else if (difficulty == "normal")
            return data.normal;
        else if (difficulty == "hard")
            return data.hard;
        else
            return 0;
    }

    // Salva in AUTOMATICO qualità, easy, normal e hard
    public void SaveGame()
    {
        Save(new SerializableSaveData(stats.easy, stats.normal, stats.hard, quality.value), "SaveData");
    }

    public bool SaveGameExists()
    {
        return File.Exists(Application.dataPath + "/saves/SaveData.dat");
    }

    /* TESTING */

    public int easy = 0;
    public int normal = 0;
    public int hard = 0;
    public int cameraQualityLevel = 0;
    public string name = "";

    public void SaveFileFromInspector()
    {
        Save(new SerializableSaveData(easy, normal, hard, cameraQualityLevel), name);
    }

    public void LoadFileFromInspector()
    {
        SerializableSaveData data = Load(name);

        if (data == null)
        {
            print("Save not found");
            return;
        }

        easy = data.easy;
        normal = data.normal;
        hard = data.hard;
        cameraQualityLevel = data.cameraQuality;

        stats.easy = data.easy;
        stats.normal = data.normal;
        stats.hard = data.hard;
        quality.value = data.cameraQuality;
    }
}
