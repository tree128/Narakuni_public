using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "MyScriptableObjects/FlagData", fileName = "FlagData")]
public class FlagData : ScriptableObject
{
    [Serializable]
    public class FlagClass
    {
        public string name;
        [Readonly]public int number;
        public bool flag;
    }

    [Serializable]
    public class FlagListClass
    {
        public List<FlagClass> flags;
    }

    public FlagListClass flagList;

    private void OnValidate()
    {
        if(0 < flagList.flags.Count)
        {
            FlagClass lastFlag = flagList.flags[flagList.flags.Count - 1];
            string previousName = "";
            if (2 <= flagList.flags.Count)
            {
                previousName = flagList.flags[flagList.flags.Count - 2].name;
            }
            if (lastFlag.name == previousName)// 追加されたときフラグナンバーの重複がないようにする
            {
                lastFlag.flag = false;
                int[] numArray = new int[flagList.flags.Count];
                int count = 0;
                foreach (var item in flagList.flags)
                {
                    numArray[count] = item.number;
                }
                Array.Sort(numArray);
                lastFlag.number = numArray[numArray.Length - 1] + 1;
                lastFlag.name = "Flag" + lastFlag.number.ToString("000");
            }
        }
        List<string> nameList = new List<string>();
        foreach (var item in flagList.flags)
        {
            if (nameList.Contains(item.name))
            {
                Debug.LogError("フラグ名に重複があります。修正してください。");
                break;
            }
            else
            {
                nameList.Add(item.name);
            }
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void GameStart_SaveDataLoad()
    {
        string filePath = Application.persistentDataPath + "/SaveData_Flag.json";
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            string info = reader.ReadToEnd();
            reader.Close();
            FlagListClass flagList = GetFlagDataAsset().flagList;
            flagList.flags = JsonUtility.FromJson<FlagListClass>(info).flags;
            Debug.Log("ゲーム開始：フラグのセーブデータをロードしました");
        }
    }

    /// <summary>
    /// フラグをTrueにする
    /// </summary>
    /// <param name="flagNum">対象のフラグナンバー</param>
    public void SetFlag_True(int flagNum)
    {
        foreach (var item in flagList.flags)
        {
            if(item.number == flagNum)
            {
                item.flag = true;
                Save();
                break;
            }
        }
    }

    /// <summary>
    /// フラグをFalseにする
    /// </summary>
    /// <param name="flagNum">対象のフラグナンバー</param>
    public void SetFlag_False(int flagNum)
    {
        foreach (var item in flagList.flags)
        {
            if (item.number == flagNum)
            {
                item.flag = false;
                Save();
                break;
            }
        }
    }

    /// <summary>
    /// 引数で指定したナンバーのフラグがあれば返す。
    /// </summary>
    /// <param name="flagNum">対象のフラグナンバー</param>
    /// <returns>FlagClass　もしくは null</returns>
    public FlagClass GetFlag(int flagNum)
    {
        foreach (var item in flagList.flags)
        {
            if(item.number == flagNum)
            {
                return item;
            }
        }

        return null;
    }

#if UNITY_EDITOR
    public static FlagData GetFlagDataAsset()
    {
        string flagDataGUID = AssetDatabase.FindAssets("FlagData", new string[1] { "Assets/Data" })[0];
        string path = AssetDatabase.GUIDToAssetPath(flagDataGUID);
        FlagData asset = AssetDatabase.LoadAssetAtPath(path, typeof(FlagData)) as FlagData;
        return asset;
    }
#endif

    public void Save()
    {
        string info = JsonUtility.ToJson(flagList);
        string filePath = Application.persistentDataPath + "/SaveData_Flag.json";
        StreamWriter writer = new StreamWriter(filePath, false); // false:上書き保存
        writer.WriteLine(info);
        writer.Close();
        Debug.Log("フラグをセーブしました");
    }

    public void Load()
    {
        string filePath = Application.persistentDataPath + "/SaveData_Flag.json";
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            string info = reader.ReadToEnd();
            reader.Close();
            flagList = JsonUtility.FromJson<FlagListClass>(info);
            Debug.Log("フラグをロードしました");
        }
    }

    public void SetAllTrue()
    {
        Load();
        foreach (var item in flagList.flags)
        {
            item.flag = true;
        }
        Debug.Log("全てTrueにしました");
        Save();
    }

    public void SetAllFalse()
    {
        Load();
        foreach (var item in flagList.flags)
        {
            item.flag = false;
        }
        Debug.Log("全てFalseにしました");
        Save();
    }
}
