using UnityEngine;
using System.Collections.Generic;

public class GPSMelodyClueManager : MonoBehaviour
{
    public static GPSMelodyClueManager Instance { get; private set; }

    public bool HasGPSClue { get; private set; }
    public string ClueTitle { get; private set; }
    public string MelodyCode { get; private set; }
    public string FoundMessage { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetGPSClue(string clueTitle, string melodyCode, string foundMessage)
    {
        HasGPSClue = true;
        ClueTitle = clueTitle;
        MelodyCode = melodyCode;
        FoundMessage = foundMessage;

        Debug.Log($"GPS clue saved: {MelodyCode}");
    }

    public int[] GetMelodyDigits()
    {
        List<int> digits = new List<int>();

        if (string.IsNullOrEmpty(MelodyCode))
            return digits.ToArray();

        foreach (char c in MelodyCode)
        {
            if (char.IsDigit(c))
                digits.Add(c - '0');
        }

        return digits.ToArray();
    }
}