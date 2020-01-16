using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class SaveLoadHandler : MonoBehaviour
{

    #region Methods
    public static void Load(string category)
    {
        if (File.Exists(Application.persistentDataPath + "/" + "." + category))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + "." + category, FileMode.Open);
            DataSerializer ds = bf.Deserialize(stream) as DataSerializer;
            stream.Close();
            print(ds.cat + "    " + ds.someIntValue + "   " + ds.name);
        }
        else
        {
            Debug.LogError("File does not exists to load");
        }
    }

    public static void Save(string category, DataContainer dc, bool append = false)
    {
        string url = Application.persistentDataPath + "/" + "." + category;
        if (File.Exists (url))
        {
            //create new file
            if (!append)
            {
                BinaryFormatter bf = new BinaryFormatter ();
                FileStream stream = new FileStream (url, FileMode.Create);
                DataSerializer ds = new DataSerializer (dc);
                bf.Serialize (stream, ds);
                stream.Close ();
            }
            //check if file exits then append on it
            else
            {
                BinaryFormatter bf = new BinaryFormatter ();
                FileStream stream = new FileStream (url, FileMode.Open);
                DataSerializer ds = new DataSerializer (dc);
                bf.Serialize (stream, ds);
                stream.Close ();
            }
        }
        else
        {
            if (!append)
            {
                BinaryFormatter bf = new BinaryFormatter ();
                FileStream stream = new FileStream (url, FileMode.Create);
                DataSerializer ds = new DataSerializer (dc);
                bf.Serialize (stream, ds);
                stream.Close ();
            }
            else
            {
                Debug.LogError ("File does not exists to append");
            }
        }

    }
    #endregion Methods
}