using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class URGJsonManager : MonoBehaviour
{
    [Space, Header("URG Managers"), SerializeField]
    public List<URGManager> URGManagers;

    [Space, Header("Setting Folder Paths"), SerializeField]
    List<string> SettingFolderPaths;
    [Space, Header("Setting Json Paths"), SerializeField]
    List<string> SettingJsonPaths;

    [Space, Header("URG Datas"), SerializeField]
    public List<URGData> URGDatas;

    void Awake()
    {
        foreach (var(child, index) in this.transform.Cast<Transform>().Select((child, index) => (child, index)).Skip(1))
        {
            URGManagers.Add(child.GetComponent<URGManager>());
        }

        SettingFolderPaths.Add(Application.streamingAssetsPath);
        SettingFolderPaths.Add(Application.streamingAssetsPath + "/Setting Json");
        SettingFolderPaths.Add(Application.streamingAssetsPath + "/Setting Json/URG");

        foreach (var (urgmanager, index) in URGManagers.Select((urgmanager, index) => (urgmanager, index)))
        {
            SettingJsonPaths.Add(Application.streamingAssetsPath + "/Setting Json/URG/URG Data " + index + ".json");
        }

        CheckFolder();
        CheckJson();
    }

    void CheckFolder()
    {
        foreach (string folder in SettingFolderPaths)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }

    void CheckJson()
    {
        foreach (var (json, index) in SettingJsonPaths.Select((json, index) => (json, index)))
        {
            if (!File.Exists(json))
            {
                WriteJson(index);
            }
            ReadJson(json, index);
        }
    }

    public void WriteJson(int id)
    {
        URGData URGData = new URGData()
        {
            strIPAddress = URGManagers[id].strIPAddress,
            strPortNumber = URGManagers[id].iPortNumber.ToString(),
            URGPos = URGManagers[id].transform.position,
            Scale = URGManagers[id].fScale.ToString(),
            Limit = URGManagers[id].fLimit.ToString(),
            NoiseLimit = URGManagers[id].iNoiseLimit.ToString(),
            XMinPos = URGManagers[id].fXMinPos.ToString(),
            YMinPos = URGManagers[id].fYMinPos.ToString()
        };

        string settingdata = JsonUtility.ToJson(URGData);

        StreamWriter file = new StreamWriter(SettingJsonPaths[id]);
        file.Write(settingdata);
        file.Close();
    }

    public void ReadJson(string path, int id)
    {
        using (StreamReader streamreader = File.OpenText(path))
        {
            string settingdata = streamreader.ReadToEnd();
            streamreader.Close();

            URGDatas.Add(JsonUtility.FromJson<URGData>(settingdata));
        }

        URGManagers[id].strIPAddress = URGDatas[id].strIPAddress;
        URGManagers[id].iPortNumber = int.Parse(URGDatas[id].strPortNumber);
        URGManagers[id].transform.position = URGDatas[id].URGPos;
        URGManagers[id].fScale = float.Parse(URGDatas[id].Scale);
        URGManagers[id].fLimit = float.Parse(URGDatas[id].Limit);
        URGManagers[id].iNoiseLimit = int.Parse(URGDatas[id].NoiseLimit);
        URGManagers[id].fXMinPos = float.Parse(URGDatas[id].XMinPos);
        URGManagers[id].fYMinPos = float.Parse(URGDatas[id].YMinPos);
    }
}

[System.Serializable]
public class URGData
{
    public string strIPAddress;
    public string strPortNumber;
    public Vector3 URGPos;
    public string Scale;
    public string Limit;
    public string NoiseLimit;
    public string XMinPos;
    public string YMinPos;
}