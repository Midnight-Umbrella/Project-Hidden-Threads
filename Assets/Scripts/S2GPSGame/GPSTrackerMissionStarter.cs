using UnityEngine;

public class GPSTrackerMissionStarter : MonoBehaviour
{
    [SerializeField] private GPSTrackerController tracker;
    [SerializeField] private GPSClueTarget targetToTrack;

    public void StartTrackingMission()
    {
        if (tracker == null || targetToTrack == null) return;
        tracker.StartTracking(targetToTrack);
    }
}