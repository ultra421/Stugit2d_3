using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceList;
    private FileDataHandler fileDataHandler;
    public static DataPersistanceManager instance { get; private set; }

    //Called first
    private void Awake()
    { 
        if (instance != null)
        {
            Debug.LogError("Found more than one dataManagers");
        }
        instance = this;

        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

    }
     
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    //Called Third
    private void Start()
    {
        
    }

    //Called Second
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("onSceneLoad and Persistance path =" + Application.persistentDataPath);

        dataPersistenceList = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log(scene + " unloaded");
        SaveGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        //Save new Data (also saves on scene change so it's unecessary)
        fileDataHandler.Save(this.gameData);
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
