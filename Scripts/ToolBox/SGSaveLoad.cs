using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Firebase.Database;

public class SGSaveLoad
{
    public static int remoteStatus = 0; //1=authenticated; 0=none; -1=trying
    public static DataSnapshot snapshot = null;

    private static string localFilePath = "";
    private static string localFileExtension = ".dat";
    private static string[] hashSpearator = { "@=>" };
    private static string lastError = "";

    public static string LastError {
        get {
            return lastError;
        }
    }

    public static string LocalFilePath {
        get {
            if (string.IsNullOrEmpty(localFilePath))
            {
                if (SGPermissions.ExternalStorageReadPermission() && SGPermissions.ExternalStorageWritePermission())
                {
                    localFilePath = Application.persistentDataPath + "/" + Application.productName.Replace(" ", "");
                }
            }
            return localFilePath;
        }
    }

    public static bool SaveLocal(object dataSerializable)
    {
        if (string.IsNullOrEmpty(LocalFilePath))
            return false;
        
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            FileStream file = File.Create(LocalFilePath + localFileExtension); //you can call it anything you want
            bf.Serialize(file, dataSerializable);
            file.Close();
            return true;
        }
        catch (System.Exception e)
        {
            SGDebug.LogWarning("Failed to save the saved games file in " + LocalFilePath + localFileExtension);
            SGDebug.LogWarning(e);
        }

        return false;
    }

    public static T LoadLocal<T>() where T : new()
    {
        object dataDeserialize = new T();

        if (string.IsNullOrEmpty(LocalFilePath)) 
            return (T)dataDeserialize;

        if (!File.Exists(LocalFilePath + localFileExtension))
            return (T)dataDeserialize;

        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            FileStream file = File.Open(LocalFilePath + localFileExtension, FileMode.Open);
            dataDeserialize = bf.Deserialize(file);
            file.Close();
            return (T)dataDeserialize;
        }
        catch (System.Exception e)
        {
            SGDebug.LogWarning("Failed to open the saved games file in " + LocalFilePath + localFileExtension);
            SGDebug.LogWarning(e);
        }

        return new T();
    }

    public static bool DeleteLocal()
    {
        if (string.IsNullOrEmpty(LocalFilePath))
            return false;

        if (!File.Exists(LocalFilePath + localFileExtension))
            return false;

        try
        {
            File.Delete(LocalFilePath + localFileExtension);
            return true;
        }
        catch (System.Exception e)
        {
            SGDebug.LogWarning("Failed to delete the saved games file in " + LocalFilePath + localFileExtension);
            SGDebug.LogWarning(e);
        }
        
        return false;
    }

    // ref: https://docs.unity3d.com/ScriptReference/Windows.File.WriteAllBytes.html
    public static bool SaveLocalXML(object dataSerializable, string key)
    {
        string json;

        if (string.IsNullOrEmpty(LocalFilePath))
            return false;

        json = JsonUtility.ToJson(dataSerializable, false);
        if (string.IsNullOrEmpty(json))
            return false;

        // save to disc
        File.WriteAllText(LocalFilePath + key + localFileExtension, SGMd5Sum.Sign(json));

        return true;
    }

    public static T LoadLocalXML<T>(string key) where T : new()
    {
        object dataDeserialize = new T();
        string content;
        string json;

        if (string.IsNullOrEmpty(LocalFilePath))
            return (T)dataDeserialize;

        if (!File.Exists(LocalFilePath + key + localFileExtension))
            return (T)dataDeserialize;

        // load from disc
        content = File.ReadAllText(LocalFilePath + key + localFileExtension);
        if (string.IsNullOrEmpty(content))
            return (T)dataDeserialize;

        json = SGMd5Sum.Validate(content);
        if (string.IsNullOrEmpty(json))
            return (T)dataDeserialize;

        dataDeserialize = JsonUtility.FromJson<T>(json);

        return (T)dataDeserialize;
    }

    public static void UploadXML(object dataSerializable, string key)
    {
        DatabaseReference referenceRTD = null;
        string json;

        if (remoteStatus == -1)
            return;

        if (string.IsNullOrEmpty(SGFirebase.userId))
            return;
        
        json = JsonUtility.ToJson(dataSerializable, false);
        if (string.IsNullOrEmpty(json))
            return;

        // save into remote dabatase
        referenceRTD = SGFirebase.RealTimeDatabase("saved-games/" + SGFirebase.userId + "/" + key);
        if (referenceRTD == null)
            return;
        
        remoteStatus = -1;
        lastError = "";
        
        referenceRTD.SetValueAsync(json).ContinueWith(task => {
            if (task.IsCanceled)
            {
                lastError = task.Exception.ToString();
                remoteStatus = 0;
                return;
            }
            if (task.IsFaulted)
            {
                lastError = task.Exception.InnerExceptions[0].InnerException.Message;
                remoteStatus = 0;
                return;
            }

            if (task.IsCompleted)
            {
                remoteStatus = 1;
                return;
            }
        });

        return;
    }

    public static void RemoveXML(string key)
    {
        DatabaseReference referenceRTD = null;

        if (remoteStatus == -1)
            return;

        if (string.IsNullOrEmpty(SGFirebase.userId))
            return;

        // save into remote dabatase
        referenceRTD = SGFirebase.RealTimeDatabase("saved-games/" + SGFirebase.userId + "/" + key);
        if (referenceRTD == null)
            return;

        remoteStatus = -1;
        lastError = "";

        referenceRTD.RemoveValueAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                lastError = task.Exception.ToString();
                remoteStatus = 0;
                return;
            }
            if (task.IsFaulted)
            {
                lastError = task.Exception.InnerExceptions[0].InnerException.Message;
                remoteStatus = 0;
                return;
            }

            if (task.IsCompleted)
            {
                remoteStatus = 1;
                return;
            }
        });

        return;
    }

    public static void DownloadXMLs()
    {
        DatabaseReference referenceRTD = null;

        if (remoteStatus == -1)
            return;

        if (string.IsNullOrEmpty(SGFirebase.userId))
            return;

        // remote dabatase
        referenceRTD = SGFirebase.RealTimeDatabase("saved-games/" + SGFirebase.userId);
        if (referenceRTD == null)
            return;

        remoteStatus = -1;
        lastError = "";
        snapshot = null;

        referenceRTD.GetValueAsync().ContinueWith(task =>{
            if (task.IsCanceled)
            {
                lastError = task.Exception.ToString();
                remoteStatus = 0;
                return;
            }
            if (task.IsFaulted)
            {
                lastError = task.Exception.InnerExceptions[0].InnerException.Message;
                remoteStatus = 0;
                return;
            }
            if (task.IsCompleted)
            {
                remoteStatus = 1;
                snapshot = task.Result;
                return;
            }
        });
    }

    public static void ConvertXMLToObject(object dataSerializable, object json)
    {
        JsonUtility.FromJsonOverwrite(json.ToString(), dataSerializable);
    }
}
