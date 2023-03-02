using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceList;
    private FileDataHandler fileDataHandler;
    public static DataPersistanceManager instance { get; private set; }

    private void Awake()
    { 
        if (instance != null)
        {
            Debug.LogError("Found more than one dataManagers");
        }
        instance = this;
    }

    private void Start()
    {
        Debug.Log("Persistance path =" + Application.persistentDataPath);
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceList = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();

        //Load save data from a file
        if (this.gameData == null)
        {
            Debug.Log("New game created");
            NewGame();
        }

        foreach(IDataPersistence persObj in dataPersistenceList)
        {
            persObj.LoadData(gameData);
        }
        Debug.Log("Loaded gameData coins = " + gameData.coinCount);
    }

    public void SaveGame()
    {
        //Get the data from the other scripts that implement to save
        foreach (IDataPersistence persObj in dataPersistenceList)
        {
            Debug.Log("Saving " + persObj);
            persObj.SaveData(gameData);
        }
        Debug.Log("Saved gameData coins = " + gameData.coinCount);

        //Save the data to a file
        fileDataHandler.Save(gameData);

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects ()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjectList = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjectList);
    }

}
