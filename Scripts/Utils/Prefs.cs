using System;
using UnityEngine;

public static class Prefs
{
    private const string ArrayLengthKey = "LENGTH";
    private const string VectorXKey = "X";
    private const string VectorYKey = "Y";
    private const string VectorZKey = "Z";

    public static int LoadVariable(string key, int defaultValue)
    {
        return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : defaultValue;
    }

    public static float LoadVariable(string key, float defaultValue)
    {
        return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : defaultValue;
    }

    public static void SaveVariable(int variable, string key)
    {
        PlayerPrefs.SetInt(key, variable);
    }

    public static void SaveVariable(float variable, string key)
    {
        PlayerPrefs.SetFloat(key, variable);
    }

    public static void SaveArray(int[] array, string key)
    {
        for (int i = 0; i < array.Length; i++)
        {
            PlayerPrefs.SetInt(key + i, array[i]);
        }

        PlayerPrefs.SetInt(key + ArrayLengthKey, array.Length);
    }

    public static void SaveArray(float[] array, string key)
    {
        for (int i = 0; i < array.Length; i++)
        {
            PlayerPrefs.SetFloat(key + i, array[i]);
        }
    }

    public static void SaveArray(string[] array, string key)
    {
        for (int i = 0; i < array.Length; i++)
        {
            PlayerPrefs.SetString(key + i, array[i]);
        }

        PlayerPrefs.SetInt(key + ArrayLengthKey, array.Length);
    }

    public static int[] LoadArray(string key)
    {
        var length = 0;
        int[] array;

        if (PlayerPrefs.HasKey(key + ArrayLengthKey))
        {
            length = PlayerPrefs.GetInt(key + ArrayLengthKey);
        }
        else
        {
            array = Array.Empty<int>();
            return array;
        }

        array = new int[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = PlayerPrefs.GetInt(key + i);
        }

        return array;
    }

    public static void LoadArray(out string[] array, string key)
    {
        int length;

        if (PlayerPrefs.HasKey(key + ArrayLengthKey))
        {
            length = PlayerPrefs.GetInt(key + ArrayLengthKey);
        }
        else
        {
            array = Array.Empty<string>();
            return;
        }

        array = new string[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = PlayerPrefs.GetString(key + i);
        }
    }

    public static void LoadArray(out int[] array, string key, int defaultValue, int length)
    {
        int loadedLength;

        if (PlayerPrefs.HasKey(key + ArrayLengthKey))
        {
            loadedLength = PlayerPrefs.GetInt(key + ArrayLengthKey);
        }
        else
        {
            array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = defaultValue;
            }

            return;
        }

        array = new int[loadedLength];
        for (int i = 0; i < loadedLength; i++)
        {
            array[i] = PlayerPrefs.GetInt(key + i);
        }
    }

    public static void SaveVariable(Vector3 variable, string key)
    {
        PlayerPrefs.SetFloat(key + VectorXKey, variable.x);
        PlayerPrefs.SetFloat(key + VectorYKey, variable.y);
        PlayerPrefs.SetFloat(key + VectorZKey, variable.z);
    }

    public static void LoadVariable(out Vector3 variable, string key, Vector3 defaultValue)
    {
        float x = defaultValue.x, y = defaultValue.y, z = defaultValue.z;
        if (PlayerPrefs.HasKey(key + VectorXKey))
        {
            x = PlayerPrefs.GetFloat(key + VectorXKey);
        }

        if (PlayerPrefs.HasKey(key + VectorYKey))
        {
            y = PlayerPrefs.GetFloat(key + VectorYKey);
        }

        if (PlayerPrefs.HasKey(key + VectorZKey))
        {
            z = PlayerPrefs.GetFloat(key + VectorZKey);
        }

        variable = new Vector3(x, y, z);
    }

    public static void SaveArray(Vector3[] array, string key)
    {
        for (int i = 0; i < array.Length; i++)
        {
            SaveVariable(array[i], key + i);
        }

        PlayerPrefs.SetInt(key + ArrayLengthKey, array.Length);
    }

    public static void LoadArray(out Vector3[] array, string key)
    {
        int length;

        Debug.Log(PlayerPrefs.HasKey(key + ArrayLengthKey));
        Debug.Log(key + ArrayLengthKey);
        if (PlayerPrefs.HasKey(key + ArrayLengthKey))
        {
            length = PlayerPrefs.GetInt(key + ArrayLengthKey);
        }
        else
        {
            array = Array.Empty<Vector3>();
            return;
        }

        array = new Vector3[length];
        for (int i = 0; i < length; i++)
        {
            LoadVariable(out Vector3 element, key + i, default);
            array[i] = element;
        }
    }
}
