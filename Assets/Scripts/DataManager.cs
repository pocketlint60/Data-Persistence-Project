using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public MainManager mManager;

    // Start is called before the first frame update
    void Start()
    {
        mManager.WriteHighScore();
        LoadBest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    class SaveData
    {
        public string bestName;
        public int bestScore;
    }

    public void SaveBest()
    {
        SaveData bestData = new SaveData();
        bestData.bestName = mManager.m_bestName;
        bestData.bestScore = mManager.h_Score;

        string json = JsonUtility.ToJson(bestData);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBest()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData bestData = JsonUtility.FromJson<SaveData>(json);

            mManager.m_bestName = bestData.bestName;
            mManager.h_Score = bestData.bestScore;
        }
    }
}
