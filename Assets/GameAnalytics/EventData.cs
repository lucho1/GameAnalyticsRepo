
using System;
using System.Globalization;
using UnityEngine;

public class EventData
{
    protected uint _event_id;
    protected DateTime _timestamp;
    protected uint n_columns = 2;
    protected CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();

    public EventData()
    {
        _event_id = 0;
        _timestamp = System.DateTime.Now;
        culture.NumberFormat.NumberDecimalSeparator = ".";
        culture.NumberFormat.NumberGroupSeparator = ",";
        culture.NumberFormat.NumberDecimalDigits = 6;
    }

    public EventData(uint event_id, DateTime timestamp) 
    {
        _event_id = event_id;
        _timestamp = timestamp;
        culture.NumberFormat.NumberDecimalSeparator = ".";
        culture.NumberFormat.NumberGroupSeparator = ",";
        culture.NumberFormat.NumberDecimalDigits = 6;

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

    public string[] GetSerializedHeader()
    {
        string[] ret = new string[n_columns];
        ret[0] = "EventID";
        ret[1] = "Timestamp";

        return ret;
    }

    public string GetSerializedString() 
    {
        return _event_id.ToString() + ',' + _timestamp.ToString();
    }

    public string GetSerializedHeaderString()
    {
        string ret = "EventID,Timestamp";

        return ret;
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

    public uint GetColumns()
    {
        return n_columns;
    }
}

public class DamageEvent : EventData 
{
    Vector3 _position;
    string _type;
    string _what;

    public DamageEvent() : base()
    {
        _type = "";
        _what = "";
        _position = Vector3.up;
        n_columns = 7;
    }

    public DamageEvent(uint event_id, DateTime timestamp, Vector3 position, string type, string what = "") : base(event_id, timestamp)
    {
        _position = position;
        _type = type;
        _what = what;
        n_columns = 7;
    }

    new public string[] GetSerialized() 
    {
        string[] ret = new string[n_columns];

        ret[0] = _event_id.ToString();
        ret[1] = _type;
        ret[2] = _what;
        ret[3] = _position.x.ToString(culture);
        ret[4] = _position.y.ToString(culture);
        ret[5] = _position.z.ToString(culture);
        ret[6] = _timestamp.ToString();

        return ret;
    }

    new public string[] GetSerializedHeader()
    {
        string[] ret = new string[n_columns];
        ret[0] = "EventID";
        ret[1] = "Type";
        ret[2] = "What";
        ret[3] = "Position_X";
        ret[4] = "Position_Y";
        ret[5] = "Position_Z";
        ret[6] = "Timestamp";

        return ret;
    }

    new public string GetSerializedString() 
    {
        string ret = _event_id.ToString() + ',';

        ret += _type + ',';
        ret += _what + ',';
        ret += _position.x.ToString(culture) + ',';
        ret += _position.y.ToString(culture) + ',';
        ret += _position.z.ToString(culture) + ',';
        ret += _timestamp.ToString();

        return ret;
    }

    new public string GetSerializedHeaderString()
    {
        string ret = "EventID,Type,What,Position_X,Position_Y,Position_Z,Timestamp";
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
        _what = data[2];
        float.TryParse(data[3], NumberStyles.Float, culture, out _position.x);
        float.TryParse(data[4], NumberStyles.Float, culture, out _position.y);
        float.TryParse(data[5], NumberStyles.Float, culture, out _position.z);
        _timestamp = DateTime.Parse(data[6]);
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
        _what = data_fields[2];
        float.TryParse(data_fields[3], NumberStyles.Float, culture, out _position.x);
        float.TryParse(data_fields[4], NumberStyles.Float, culture, out _position.y);
        float.TryParse(data_fields[5], NumberStyles.Float, culture, out _position.z);
        _timestamp = DateTime.Parse(data_fields[6]);
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
        _what = temp._what;
        _position = temp._position;
    }
}

public class InteractionEvent : EventData 
{
    Vector3 _position;
    string _what;

    public InteractionEvent() : base()
    {
        _what = "";
        _position = Vector3.up;
        n_columns = 6;
    }

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
        ret[2] = _position.x.ToString(culture);
        ret[3] = _position.y.ToString(culture);
        ret[4] = _position.z.ToString(culture);
        ret[5] = _timestamp.ToString();

        return ret;
    }

        new public string[] GetSerializedHeader()
    {
        string[] ret = new string[n_columns];
        ret[0] = "EventID";
        ret[1] = "What";
        ret[2] = "Position_X";
        ret[3] = "Position_Y";
        ret[4] = "Position_Z";
        ret[5] = "Timestamp";

        return ret;
    }

    new public string GetSerializedString() 
    {
        string ret = _event_id.ToString() + ',';

        ret += _what + ',';
        ret += _position.x.ToString(culture) + ',';
        ret += _position.y.ToString(culture) + ',';
        ret += _position.z.ToString(culture) + ',';
        ret += _timestamp.ToString();

        return ret;
    }

    new public string GetSerializedHeaderString()
    {
        string ret = "EventID,What,Position_X,Position_Y,Position_Z,Timestamp";
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
        float.TryParse(data[2], NumberStyles.Float, culture, out _position.x);
        float.TryParse(data[3], NumberStyles.Float, culture, out _position.y);
        float.TryParse(data[4], NumberStyles.Float, culture, out _position.z);
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
        float.TryParse(data_fields[2], NumberStyles.Float, culture, out _position.x);
        float.TryParse(data_fields[3], NumberStyles.Float, culture, out _position.y);
        float.TryParse(data_fields[4], NumberStyles.Float, culture, out _position.z);
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

public class PositionEvent : EventData 
{
    Vector3 _position;

    public PositionEvent() : base()
    {
        _position = Vector3.up;
        n_columns = 5;
    }

    public PositionEvent(uint event_id, DateTime timestamp, Vector3 position) : base(event_id, timestamp)
    {
        _position = position;
        n_columns = 5;
    }

        new public string[] GetSerialized() 
    {
        string[] ret = new string[n_columns];

        ret[0] = _event_id.ToString();
        ret[1] = _position.x.ToString(culture);
        ret[2] = _position.y.ToString(culture);
        ret[3] = _position.z.ToString(culture);
        ret[4] = _timestamp.ToString();

        return ret;
    }

        new public string[] GetSerializedHeader()
    {
        string[] ret = new string[n_columns];
        ret[0] = "EventID";
        ret[1] = "Position_X";
        ret[2] = "Position_Y";
        ret[3] = "Position_Z";
        ret[4] = "Timestamp";

        return ret;
    }

    new public string GetSerializedString() 
    {
        string ret = _event_id.ToString() + ',';

        ret += _position.x.ToString(culture) + ',';
        ret += _position.y.ToString(culture) + ',';
        ret += _position.z.ToString(culture) + ',';
        ret += _timestamp.ToString();

        return ret;
    }

    new public string GetSerializedHeaderString()
    {
        string ret = "EventID,Position_X,Position_Y,Position_Z,Timestamp";
        return ret;
    }

    new public void FromSerialized(string [] data)
    {
        if (data.Length != n_columns) {
            Debug.LogError("Wrong amount of inputs to serialize");
            return;
        }

        _event_id = uint.Parse(data[0]);
        float.TryParse(data[1], NumberStyles.Float, culture, out _position.x);
        float.TryParse(data[2], NumberStyles.Float, culture, out _position.y);
        float.TryParse(data[3], NumberStyles.Float, culture, out _position.z);
        _timestamp = DateTime.Parse(data[4]);
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
        float.TryParse(data_fields[1], NumberStyles.Float, culture, out _position.x);
        float.TryParse(data_fields[2], NumberStyles.Float, culture, out _position.y);
        float.TryParse(data_fields[3], NumberStyles.Float, culture, out _position.z);
        _timestamp = DateTime.Parse(data_fields[4]);
    }

    new public void FromJSON(string data)
    {
        PositionEvent temp = JsonUtility.FromJson<PositionEvent>(data);
        if (temp == null)
        {
            Debug.LogError("Wrong JSON");
            return;
        }

        _event_id = temp._event_id;
        _timestamp = temp._timestamp;
        _position = temp._position;
    }

}




