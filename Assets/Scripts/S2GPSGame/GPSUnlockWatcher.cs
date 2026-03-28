using System.Collections;
using UnityEngine;

public class GPSUnlockWatcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ClueJournal clueJournal;
    [SerializeField] private ClueDefinition gpsClue;
    [SerializeField] private GPSTrackerMissionStarter missionStarter;

    [Header("Options")]
    [SerializeField] private bool triggerOnlyOnce = true;
    [SerializeField] private float checkInterval = 0.25f;

    private bool hasTriggered = false;
    private Coroutine watchRoutine;

    private void Awake()
    {
        if (clueJournal == null)
            clueJournal = ClueJournal.Instance;
    }

    private void OnEnable()
    {
        watchRoutine = StartCoroutine(WatchForGPSClue());
    }

    private void OnDisable()
    {
        if (watchRoutine != null)
            StopCoroutine(watchRoutine);
    }

    private IEnumerator WatchForGPSClue()
    {
        while (true)
        {
            if ((!triggerOnlyOnce || !hasTriggered) &&
                clueJournal != null &&
                gpsClue != null &&
                missionStarter != null)
            {
                if (JournalHasGPSClue())
                {
                    missionStarter.StartTrackingMission();
                    hasTriggered = true;
                    Debug.Log("GPSUnlockWatcher: gps clue detected, tracker started.");
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private bool JournalHasGPSClue()
    {
        if (clueJournal == null || gpsClue == null) return false;
        if (string.IsNullOrWhiteSpace(gpsClue.id)) return false;

        return clueJournal.HasClue(gpsClue.id);
    }
}