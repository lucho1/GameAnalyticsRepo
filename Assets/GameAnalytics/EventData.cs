
using System;
using UnityEngine;

public class EventData : MonoBehaviour
{
    uint event_id;
    DateTime timestamp;

    public string GetJSON() 
    {
        return JsonUtility.ToJson(this);
    }

    public string[] GetSerialized() 
    {
        string[] ret = new string[2];

        ret[0] = event_id.ToString();
        ret[1] = timestamp.ToString();

        return ret;
    }

    public string GetSerializedString() 
    {
        return event_id.ToString() + ',' + timestamp.ToString();
    }
}
