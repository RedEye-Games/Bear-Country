using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighScoreSaver : MonoBehaviour
{

    public HighScoreData highScoreData;
    const string folderName = "HighScoreData";
    const string fileExtension = ".dat";

    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.S))
        {
            string folderPath = Path.Combine(Application.persistentDataPath, folderName);
            if (!Directory.Exists (folderPath))
                Directory.CreateDirectory (folderPath);            

            string dataPath = Path.Combine(folderPath, "HighScores" + fileExtension);
            SaveScores(highScoreData, dataPath);
        }

        if (Input.GetKeyDown (KeyCode.L))
        {
            string[] filePaths = GetFilePaths ();
            
            if(filePaths.Length > 0)
            {
                highScoreData = LoadScores(filePaths[0]);
            }

        }
    }

    public void SaveScore(int newScore, string newSeed = "", bool newShared = false)
    {
        string[] filePaths = GetFilePaths();

        if (filePaths.Length > 0)
        {
            highScoreData = LoadScores(filePaths[0]);
        }
        else
        {
            highScoreData = new HighScoreData();
        }
        Score scoreToAdd = new Score
        {
            score = newScore,
            seed = newSeed,
            shared = newShared
        };
        highScoreData.scores.Add(scoreToAdd);

        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string dataPath = Path.Combine(folderPath, "HighScores" + fileExtension);
        SaveScores(highScoreData, dataPath);
    }

    static void SaveScores (HighScoreData data, string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open (path, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize (fileStream, data);
        }
    }

    static HighScoreData LoadScores (string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open (path, FileMode.Open))
        {
            return (HighScoreData)binaryFormatter.Deserialize (fileStream);
        }
    }

    static string[] GetFilePaths ()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        return Directory.GetFiles (folderPath, "*"+fileExtension);
    }
}