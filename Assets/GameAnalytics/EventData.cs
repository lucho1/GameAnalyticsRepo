
using System;
using UnityEngine;

public class EventData : MonoBehaviour
{
    protected uint _event_id;
    protected DateTime _timestamp;
    protected uint n_columns = 2;

    public EventData(uint event_id, DateTime timestamp) 
    {
        _event_id = event_id;
        _timestamp = timestamp;
    }

    public string GetJSON() 
    {
        return JsonUtility.ToJson(this);
    }

    public string[] GetSerialized() 
    {
        string[] ret = new string[n_columns];

        ret[0] = _event_id.ToString();
        ret[1] = _timestamp.ToString();

        return ret;
    }

    public string GetSerializedString() 
    {
        return _event_id.ToString() + ',' + _timestamp.ToString();
    }

    public void FromSerialized(string [] data)
    {
        if (data.Length != n_columns) {
            Debug.LogError("Wrong amount of inputs to serialize");
            return;
        }

        _event_id = uint.Parse(data[0]);
        _timestamp = DateTime.Parse(data[1]);
    }

    public void FromSerializedString (string data)
    {
        string[] data_fields = data.Split(',');
         if (data_fields.Length != n_columns) 
         {
            Debug.LogError("Wrong amount of inputs to serialize");
            return;
        }

        _event_id = uint.Parse(data_fields[0]);
        _timestamp = DateTime.Parse(data_fields[1]);
    }

    public void FromJSON(string data)
    {
        EventData temp = JsonUtility.FromJson<EventData>(data);
        if (temp == null)
        {
            Debug.LogError("Wrong JSON");
            return;
        }

        _event_id = temp._event_id;
        _timestamp = temp._timestamp;
    }
}

public class DamageEvent : EventData 
{
    Vector3 _position;
    string _type;

    public DamageEvent(uint event_id, DateTime timestamp, Vector3 position, string type) : base(event_id, timestamp)
    {
        _position = position;
        _type = type;
        n_columns = 6;
    }

    new public string[] GetSerialized() 
    {
        string[] ret = new string[n_columns];

        ret[0] = _event_id.ToString();
        ret[1] = _type;
        ret[2] = _position.x.ToString();
        ret[3] = _position.y.ToString();
        ret[4] = _position.z.ToString();
        ret[5] = _timestamp.ToString();

        return ret;
    }

    new public string GetSerializedString() 
    {
        string ret = _event_id.ToString() + ',';

        ret += _type + ',';
        ret += _position.x.ToString() + ',';
        ret += _position.y.ToString() + ',';
        ret += _position.z.ToString() + ',';
        ret += _timestamp.ToString();

        return ret;
    }

    new public void FromSerialized(string [] data)
    {
        if (data.Length != n_columns) {
            Debug.LogError("Wrong amount of inputs to serialize");
            return;
        }

        _event_id = uint.Parse(data[0]);
        _type = data[1];
        _position.x = float.Parse(data[2]);
        _position.y = float.Parse(data[3]);
        _position.z = float.Parse(data[4]);
        _timestamp = DateTime.Parse(data[5]);
    }

    new public void FromSerializedString (string data)
    {
        string[] data_fields = data.Split(',');
         if (data_fields.Length != n_columns) 
         {
            Debug.LogError("Wrong amount of inputs to serialize");
            return;
        }

        _event_id = uint.Parse(data_fields[0]);
        _type = data_fields[1];
        _position.x = float.Parse(data_fields[2]);
        _position.y = float.Parse(data_fields[3]);
        _position.z = float.Parse(data_fields[4]);
        _timestamp = DateTime.Parse(data_fields[5]);
    }

    new public void FromJSON(string data)
    {
        DamageEvent temp = JsonUtility.FromJson<DamageEvent>(data);
        if (temp == null)
        {
            Debug.LogError("Wrong JSON");
            return;
        }

        _event_id = temp._event_id;
        _timestamp = temp._timestamp;
        _type = temp._type;
        _position = temp._position;
    }
}

public class InteractionEvent : EventData 
{
    Vector3 _position;
    string _what;

    public InteractionEvent(uint event_id, DateTime timestamp, Vector3 position, string what) : base(event_id, timestamp)
    {
        _position = position;
        _what = what;
        n_columns = 6;
    }

        new public string[] GetSerialized() 
    {
        string[] ret = new string[n_columns];

        ret[0] = _event_id.ToString();
        ret[1] = _what;
        ret[2] = _position.x.ToString();
        ret[3] = _position.y.ToString();
        ret[4] = _position.z.ToString();
        ret[5] = _timestamp.ToString();

        return ret;
    }

    new public string GetSerializedString() 
    {
        string ret = _event_id.ToString() + ',';

        ret += _what + ',';
        ret += _position.x.ToString() + ',';
        ret += _position.y.ToString() + ',';
        ret += _position.z.ToString() + ',';
        ret += _timestamp.ToString();

        return ret;
    }

    new public void FromSerialized(string [] data)
    {
        if (data.Length != n_columns) {
            Debug.LogError("Wrong amount of inputs to serialize");
            return;
        }

        _event_id = uint.Parse(data[0]);
        _what = data[1];
        _position.x = float.Parse(data[2]);
        _position.y = float.Parse(data[3]);
        _position.z = float.Parse(data[4]);
        _timestamp = DateTime.Parse(data[5]);
    }

    new public void FromSerializedString (string data)
    {
        string[] data_fields = data.Split(',');
         if (data_fields.Length != n_columns) 
         {
            Debug.LogError("Wrong amount of inputs to serialize");
            return;
        }

        _event_id = uint.Parse(data_fields[0]);
        _what = data_fields[1];
        _position.x = float.Parse(data_fields[2]);
        _position.y = float.Parse(data_fields[3]);
        _position.z = float.Parse(data_fields[4]);
        _timestamp = DateTime.Parse(data_fields[5]);
    }

    new public void FromJSON(string data)
    {
        InteractionEvent temp = JsonUtility.FromJson<InteractionEvent>(data);
        if (temp == null)
        {
            Debug.LogError("Wrong JSON");
            return;
        }

        _event_id = temp._event_id;
        _timestamp = temp._timestamp;
        _what = temp._what;
        _position = temp._position;
    }

}




