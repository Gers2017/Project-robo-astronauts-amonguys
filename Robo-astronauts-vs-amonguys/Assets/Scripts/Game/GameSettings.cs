using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public static string volumeKey = "volume";
    public static string mouseKey = "mouse";
    static float defaultVolume = -10f;
    static float minVolume = -80f;
    static float defaultMouseSensitivity = 0.5f;
    static float minMouseSensitivity = 0.1f;

    //Master volume
    public static void SetMasterVolume(float value)
    {
        value = Mathf.Max(minVolume, value);
        PlayerPrefs.SetFloat(volumeKey, value);
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(volumeKey, defaultVolume);
    }

    //Mouse Sensitivity
    public static void SetMouseSensitivity(float value)
    {
        value = Mathf.Max(minMouseSensitivity, value);
        PlayerPrefs.SetFloat(mouseKey, value);
    }

    public static float GetMouseSensitivity()
    {
        return PlayerPrefs.GetFloat(mouseKey, defaultMouseSensitivity);
    }
}
