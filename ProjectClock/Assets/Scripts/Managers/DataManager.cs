using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }

    public void SaveFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
        PlayerPrefs.Save();
    }

    public float GetFloat(string name, float defautlValue = 0.0f)
    {
        return PlayerPrefs.GetFloat(name, defautlValue);
    }

    public void SaveInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
        PlayerPrefs.Save();
    }

    public int GetInt(string name, int defautlValue = 0)
    {
        return PlayerPrefs.GetInt(name, defautlValue);
    }
}
