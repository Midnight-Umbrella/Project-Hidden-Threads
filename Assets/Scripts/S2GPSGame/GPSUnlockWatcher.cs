using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GPSUnlockWatcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Component inventorySource;
    [SerializeField] private ClueDefinition gpsClue;
    [SerializeField] private GPSTrackerMissionStarter missionStarter;

    [Header("Options")]
    [SerializeField] private bool triggerOnlyOnce = true;
    [SerializeField] private float checkInterval = 0.25f;

    private bool hasTriggered = false;
    private Coroutine watchRoutine;

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
                inventorySource != null &&
                gpsClue != null &&
                missionStarter != null)
            {
                if (PlayerHasGPSClue())
                {
                    missionStarter.StartTrackingMission();
                    hasTriggered = true;
                    Debug.Log("GPSUnlockWatcher: gps clue detected, tracker started.");
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private bool PlayerHasGPSClue()
    {
        object source = inventorySource;
        string gpsId = GetClueId(gpsClue);

        string[] methodNames =
        {
            "HasClue",
            "ContainsClue",
            "HasItem",
            "ContainsItem",
            "Contains",
            "Has"
        };

        foreach (string methodName in methodNames)
        {
            MethodInfo method = source.GetType().GetMethod(
                methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (method == null) continue;

            ParameterInfo[] ps = method.GetParameters();
            if (ps.Length != 1) continue;

            try
            {
                if (ps[0].ParameterType == typeof(ClueDefinition))
                {
                    object result = method.Invoke(source, new object[] { gpsClue });
                    if (result is bool b && b) return true;
                }

                if (ps[0].ParameterType == typeof(string) && !string.IsNullOrEmpty(gpsId))
                {
                    object result = method.Invoke(source, new object[] { gpsId });
                    if (result is bool b && b) return true;
                }
            }
            catch { }
        }

        string[] memberNames =
        {
            "clues",
            "items",
            "inventory",
            "collectedClues",
            "ownedClues",
            "definitions"
        };

        foreach (string memberName in memberNames)
        {
            FieldInfo field = source.GetType().GetField(
                memberName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (field != null)
            {
                object value = field.GetValue(source);
                if (CollectionContainsGPS(value)) return true;
            }

            PropertyInfo prop = source.GetType().GetProperty(
                memberName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (prop != null && prop.CanRead)
            {
                object value = prop.GetValue(source);
                if (CollectionContainsGPS(value)) return true;
            }
        }

        return false;
    }

    private bool CollectionContainsGPS(object collectionObj)
    {
        if (collectionObj == null) return false;

        if (collectionObj is IEnumerable enumerable)
        {
            foreach (object item in enumerable)
            {
                if (item == null) continue;

                if (item is ClueDefinition clueDef)
                {
                    if (clueDef == gpsClue) return true;

                    string itemId = GetClueId(clueDef);
                    string gpsId = GetClueId(gpsClue);

                    if (!string.IsNullOrEmpty(itemId) && itemId == gpsId) return true;
                }

                if (TryMatchNestedItem(item)) return true;
            }
        }

        return false;
    }

    private bool TryMatchNestedItem(object item)
    {
        System.Type t = item.GetType();
        string gpsId = GetClueId(gpsClue);

        string[] idNames = { "id", "Id" };
        foreach (string idName in idNames)
        {
            FieldInfo f = t.GetField(idName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null)
            {
                object value = f.GetValue(item);
                if (value is string s && s == gpsId) return true;
            }

            PropertyInfo p = t.GetProperty(idName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.CanRead)
            {
                object value = p.GetValue(item);
                if (value is string s && s == gpsId) return true;
            }
        }

        string[] clueNames = { "clue", "definition", "clueDefinition" };
        foreach (string clueName in clueNames)
        {
            FieldInfo f = t.GetField(clueName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null)
            {
                object value = f.GetValue(item);
                if (value is ClueDefinition clueDef)
                {
                    if (clueDef == gpsClue) return true;

                    string itemId = GetClueId(clueDef);
                    if (!string.IsNullOrEmpty(itemId) && itemId == gpsId) return true;
                }
            }

            PropertyInfo p = t.GetProperty(clueName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null && p.CanRead)
            {
                object value = p.GetValue(item);
                if (value is ClueDefinition clueDef)
                {
                    if (clueDef == gpsClue) return true;

                    string itemId = GetClueId(clueDef);
                    if (!string.IsNullOrEmpty(itemId) && itemId == gpsId) return true;
                }
            }
        }

        return false;
    }

    private string GetClueId(ClueDefinition clue)
    {
        if (clue == null) return null;

        System.Type t = clue.GetType();

        FieldInfo idFieldLower = t.GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (idFieldLower != null)
        {
            object value = idFieldLower.GetValue(clue);
            if (value is string s) return s;
        }

        FieldInfo idFieldUpper = t.GetField("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (idFieldUpper != null)
        {
            object value = idFieldUpper.GetValue(clue);
            if (value is string s) return s;
        }

        PropertyInfo idPropLower = t.GetProperty("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (idPropLower != null && idPropLower.CanRead)
        {
            object value = idPropLower.GetValue(clue);
            if (value is string s) return s;
        }

        PropertyInfo idPropUpper = t.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (idPropUpper != null && idPropUpper.CanRead)
        {
            object value = idPropUpper.GetValue(clue);
            if (value is string s) return s;
        }

        return null;
    }
}