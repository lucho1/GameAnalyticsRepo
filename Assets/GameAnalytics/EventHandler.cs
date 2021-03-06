using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventHandler : MonoBehaviour
{
    [System.NonSerialized]
    public List<DamageEvent> damage_events;
    [System.NonSerialized]
    public List<InteractionEvent> interaction_events;
    [System.NonSerialized]
    public List<PositionEvent> position_events;

    float last_position_event;

    public string damageEventsFilename = "DamageEvents";
    public string interactionEventsFilename = "InteractionEvents";
    public string positionEventsFilename = "PositionEvents";
    public bool recordSession = false;
    public bool recoverSession = true;
    public int secondsForPositionEvent = 5;
    public GameObject player;

    public UnityEvent GADeathEvent, GADamageEvent, GAKillEvent, GAPositionEvent, GAInteractionEvent;



    // Credit to John Skeet for this utility function
    static T[,] CreateRectangularArray<T>(List<T[]> arrays)
    {
        // TODO: Validation and special-casing for arrays.Count == 0
        int minorLength = arrays[0].Length;
        T[,] ret = new T[arrays.Count, minorLength];
        for (int i = 0; i < arrays.Count; i++)
        {
            var array = arrays[i];
            if (array.Length != minorLength)
            {
                throw new ArgumentException
                    ("All arrays must be the same length");
            }
            for (int j = 0; j < minorLength; j++)
            {
                ret[i, j] = array[j];
            }
        }
        return ret;
    }

    void Start()
    {
        damage_events       = new List<DamageEvent>();
        interaction_events  = new List<InteractionEvent>();
        position_events     = new List<PositionEvent>();
        last_position_event = Time.time;

        if (recoverSession)
        {
            ReadDamageEvents();
            ReadInteractionEvents();
            ReadPositionEvents();
        }
    }

    void Update()
    {
        if (Time.time - last_position_event > secondsForPositionEvent)
        {
            last_position_event = Time.time;
            CreatePositionEvent(player);
        }
    }

    void OnApplicationQuit()
    {
        if (recordSession)
        {
            WriteDamageEvents();
            WriteInteractionEvents();
            WritePositionEvents();
        }
    }

    void WriteDamageEvents()
    {
        if (damage_events.Count <= 0)
            return;
        
        List<string[]> data = new List<string[]>();

        // First we add the header
        data.Add(damage_events[0].GetSerializedHeader());

        foreach (DamageEvent row in damage_events)
            data.Add(row.GetSerialized());

        string[,] serialized_data = CreateRectangularArray<string>(data);
        Writer.Write(damageEventsFilename, serialized_data);

    }

    void ReadDamageEvents()
    {
        string[][] data = Reader.Read(damageEventsFilename);

        if (data == null)
            return;

        uint nrows = (uint) data.GetLength(0);
        for (int row = 1; row < nrows; ++row) 
        {
            DamageEvent n_event = new DamageEvent();
            n_event.FromSerialized(data[row]);
            damage_events.Add(n_event);
        }
    }

    void WriteInteractionEvents()
    {
        if (interaction_events.Count <= 0)
            return;
        
        List<string[]> data = new List<string[]>();

        // First we add the header
        data.Add(interaction_events[0].GetSerializedHeader());

        foreach (InteractionEvent row in interaction_events)
            data.Add(row.GetSerialized());

        string[,] serialized_data = CreateRectangularArray<string>(data);
        Writer.Write(interactionEventsFilename, serialized_data);
    }

    void ReadInteractionEvents()
    {
        string[][] data = Reader.Read(interactionEventsFilename);

        if (data == null)
            return;

        uint nrows = (uint) data.GetLength(0);
        for (int row = 1; row < nrows; ++row) 
        {
            InteractionEvent n_event = new InteractionEvent();
            n_event.FromSerialized(data[row]);
            interaction_events.Add(n_event);
        }
    }

    void WritePositionEvents()
    {
        if (position_events.Count <= 0)
            return;
        
        List<string[]> data = new List<string[]>();

        // First we add the header
        data.Add(position_events[0].GetSerializedHeader());

        foreach (PositionEvent row in position_events)
            data.Add(row.GetSerialized());

        string[,] serialized_data = CreateRectangularArray<string>(data);
        Writer.Write(positionEventsFilename, serialized_data);
    }

    void ReadPositionEvents()
    {
        string[][] data = Reader.Read(positionEventsFilename);

        if (data == null)
            return;

        uint nrows = (uint) data.GetLength(0);
        for (int row = 1; row < nrows; ++row) 
        {
            PositionEvent n_event = new PositionEvent();
            n_event.FromSerialized(data[row]);
            position_events.Add(n_event);
        }
    }

    public void CreateDeathEvent(GameObject player)
    {
        GADeathEvent.Invoke();
        DamageEvent new_event = new DamageEvent((uint)damage_events.Count, System.DateTime.Now, player.transform.position, "Death");
        damage_events.Add(new_event);
    }

    public void CreateDamageEvent(GameObject player)
    {
        GADamageEvent.Invoke();
        DamageEvent new_event = new DamageEvent((uint)damage_events.Count, System.DateTime.Now, player.transform.position, "Damage");
        damage_events.Add(new_event);
    }

    public void CreateKillEvent(GameObject what)
    {
        GAKillEvent.Invoke();
        DamageEvent new_event = new DamageEvent((uint)damage_events.Count, System.DateTime.Now, what.transform.position, "Kill", what.name);
        damage_events.Add(new_event);
    }

    public void CreateInteractionEvent(GameObject what)
    {
        GAInteractionEvent.Invoke();
        InteractionEvent new_event = new InteractionEvent((uint)interaction_events.Count, System.DateTime.Now, what.transform.position, what.name);
        interaction_events.Add(new_event);
    }

    public void CreatePositionEvent(GameObject player)
    {
        GAPositionEvent.Invoke();
        PositionEvent new_event = new PositionEvent((uint)position_events.Count, System.DateTime.Now, player.transform.position);
        position_events.Add(new_event);
    }


}