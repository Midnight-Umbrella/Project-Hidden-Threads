using System;
using UnityEngine;

public static class VolumeBus
{
    private const string PrefKey = "master_volume";
    public static event Action<float> OnChanged;

    public static float Get()
    {
        return PlayerPrefs.GetFloat(PrefKey, 1f);
    }

    public static void Set(float v)
    {
        v = Mathf.Clamp01(v);

        PlayerPrefs.SetFloat(PrefKey, v);
        PlayerPrefs.Save();

        AudioListener.volume = v;
        OnChanged?.Invoke(v);
    }

    public static void ApplySaved()
    {
        AudioListener.volume = Get();
    }
}
