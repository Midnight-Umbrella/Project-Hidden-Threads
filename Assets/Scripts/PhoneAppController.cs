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
    {
        if (currentApp != null)
            currentApp.SetActive(false);

        homeScreen.SetActive(false);
        app.SetActive(true);
        currentApp = app;
    }

    public void GoHome()
    {
        if (currentApp != null)
            currentApp.SetActive(false);

        homeScreen.SetActive(true);
        currentApp = null;
    }
}
