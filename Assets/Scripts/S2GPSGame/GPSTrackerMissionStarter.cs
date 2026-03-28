using UnityEngine;

public class GPSTrackerMissionStarter : MonoBehaviour
{
    [SerializeField] private GPSTrackerController tracker;
    [SerializeField] private GPSClueTarget targetToTrack;

    private bool hasStarted = false;

    public void StartTrackingMission()
    {
        if (hasStarted) return;
        if (tracker == null || targetToTrack == null) return;

        tracker.StartTracking(targetToTrack);
        hasStarted = true;

        Debug.Log("GPS tracking mission started.");
    }

    public bool HasStarted()
    {
        return hasStarted;
    }
}