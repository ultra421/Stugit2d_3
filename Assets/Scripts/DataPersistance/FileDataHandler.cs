using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileId)
    {
        //Loads the data from the folder/save
        string fullPath = Path.Combine(dataDirPath,profileId, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //Deserialize the data
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.Log("Error at  Load " + e.ToString());
            }
        } else
        {
            Debug.Log("File didn't exist at " + fullPath);
        }
        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            //Create the directory where the file will be saved to
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize the data
            string dataToStore = JsonUtility.ToJson(data, true);

            Debug.Log("Saving into " + fullPath);

            //Write it into a file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                    Debug.Log("Saved sucesfully!");
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log("Error when save " + e.ToString());
        }
    }

    public Dictionary<string, GameData> LoadAllProfles()
    {
        Dictionary<string, GameData> profileDicitonary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(dataDirPath,profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Profile didn't contain a save file!");
                continue;
            }

            GameData profileData = Load(profileId);

            if (profileData != null)
            {
                profileDicitonary.Add(profileId,profileData);
            } else
            {
                Debug.LogError("Couldn't load data from some reaosn");
            }
        }
        return profileDicitonary;
    }
}
