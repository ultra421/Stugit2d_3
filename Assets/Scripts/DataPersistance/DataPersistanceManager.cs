using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceList;
    private FileDataHandler fileDataHandler;
    private string selectedProfileId = "1";
    public static DataPersistanceManager instance { get; private set; }

    //Called first
    private void Awake()
    { 
        
        if (instance != null)
        {
            Debug.LogError("Found more than one dataManagers " + instance.GetInstanceID() + " the new one is self-destroying " + this.gameObject.GetInstanceID());
            Destroy(this.gameObject);
            return;
        } else
        {
            Debug.Log("instance was null");
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

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

        Debug.Log("Loading dataManager" + this.GetInstanceID() + "instance is = " + instance.GetInstanceID());
        dataPersistenceList = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
        Debug.Log("Scene " + scene.GetHashCode() + " closed  wtih dataManager instance " + DataPersistanceManager.instance.GetInstanceID());
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        //Save new Data (also saves on scene change so it's unecessary)
        fileDataHandler.Save(this.gameData, selectedProfileId);
    }
    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load(selectedProfileId);

        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        /*//Load save data from a file
        if (this.gameData == null)
        {
            Debug.Log("Loaded data is null, A new game needs to be created");
            return;
        } */

        foreach(IDataPersistence persObj in dataPersistenceList)
        {
            persObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A new Game needs to be started before data can be saved");
            return;
        }

        //Get the data from the other scripts that implement to save
        foreach (IDataPersistence persObj in dataPersistenceList)
        {
            Debug.Log("Saving " + persObj);
            persObj.SaveData(gameData);
        }
        Debug.Log("Saved gameData coins = " + gameData.coinCount);

        //Save the data to a file
        fileDataHandler.Save(gameData, selectedProfileId);

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

    public bool hasGameData ()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return fileDataHandler.LoadAllProfles();
    }
    
    /* Reloads all the data
     */
    public void SetProfileId(int id)
    {
        selectedProfileId = id.ToString();
        //Update the loaded data
        dataPersistenceList = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public string getProfileId()
    {
        return selectedProfileId;
    }

}
