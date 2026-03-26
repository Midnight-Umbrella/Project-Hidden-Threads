using UnityEngine;

public class TrackerMissionStarter : MonoBehaviour
{
    [SerializeField] private SignalTrackerController tracker;
    [SerializeField] private SignalTarget targetToTrack;

    public void StartTrackingMission()
    {
        if (tracker == null || targetToTrack == null) return;

        tracker.StartTracking(targetToTrack);
    }
}