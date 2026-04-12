using UnityEngine;
using System.Collections.Generic;

public class PhoneAppController : MonoBehaviour
{
    public GameObject homeScreen;
    public List<GameObject> apps;

    GameObject currentApp;

    void Start()
    {
        GoHome();
    }

    public void OpenApp(GameObject app)
    {Debug.Log("Opened" + app.name);
        if (currentApp != null)
            currentApp.SetActive(false);

        homeScreen.SetActive(false);
        app.SetActive(true);
        currentApp = app;

        // Show tracker if tracking is active and maps app was opened
        if (app.name == "MapsApp")
        {
            GPSTrackerController tracker = app.GetComponentInChildren<GPSTrackerController>(true);
            if (tracker != null && tracker.IsTrackingActive())
                tracker.GetTrackerPanel()?.SetActive(true);
        }
    }

    public void GoHome()
    {Debug.Log("Home");
        if (currentApp != null)
            currentApp.SetActive(false);

        homeScreen.SetActive(true);
        currentApp = null;
    }
}
