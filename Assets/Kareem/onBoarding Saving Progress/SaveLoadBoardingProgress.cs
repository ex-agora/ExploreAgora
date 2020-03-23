using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;


public class Container
{
    public bool[] LevelIndicators;
    public bool[] LevelPrizeIndicators;
    public int LevelIndex;
    public OnBoardingPhases boardingPhases;
}

public class SaveLoadBoardingProgress 
{

    public static void Save(AppManager dc)
    {
        string url = Application.persistentDataPath + "/OnBoarding" + ".File";
        if (!File.Exists(url))
        {
            Debug.Log("File Not Exists");
            //create new file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(url, FileMode.Create);
            OnBoardingDataSerializer ds = new OnBoardingDataSerializer(dc);
           // Debug.Log(ds.currentBoardingIndex + "  " + ds.doneIndicators.Length); 
            bf.Serialize(stream, ds);
            stream.Close();
        }
        else
        {
            Debug.Log("File Exists");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream openStream = new FileStream(url, FileMode.Open);
            OnBoardingDataSerializer old = bf.Deserialize(openStream) as OnBoardingDataSerializer;
            OnBoardingDataSerializer current = new OnBoardingDataSerializer(dc);
            old.currentBoardingIndex = current.currentBoardingIndex;
            old.doneIndicators = current.doneIndicators;
            old.donePrizeIndicators = current.donePrizeIndicators;
            old.boardingPhases = current.boardingPhases;
            openStream.Close();
            FileStream appendStream = new FileStream(url, FileMode.Create);

            bf.Serialize(appendStream, old);
            appendStream.Close();

        }

    }
    public static Container Load()
    {
        Container callBackData = new Container();
        if (File.Exists(Application.persistentDataPath + "/OnBoarding" + ".File"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/OnBoarding" + ".File", FileMode.Open);
            OnBoardingDataSerializer ds = bf.Deserialize(stream) as OnBoardingDataSerializer;
            stream.Close();
            callBackData.LevelIndex = ds.currentBoardingIndex;
            callBackData.LevelIndicators = ds.doneIndicators;
            callBackData.LevelPrizeIndicators = ds.donePrizeIndicators;
            callBackData.boardingPhases = ds.boardingPhases;

            return callBackData;
        }
        else
        {
            Debug.LogWarning("File does not exists to load");
            return null;
        }
    }

public static void Delete()
    {
        File.Delete(Application.persistentDataPath + "/OnBoarding" + ".File");
    }
}

