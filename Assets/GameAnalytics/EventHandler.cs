using System;
using System.Collections.Generic;
using UnityEngine;


public class EventHandler : MonoBehaviour
{
    List<DamageEvent> damage_events;
    List<InteractionEvent> interaction_events;
    Writer writer;
    Reader reader;

    public string damage_events_filename = "DamageEvents";
    public string interaction_events_filename = "InteractionEvents";
    public bool record_session = false;
    public bool recover_session = true;


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
        writer              = new Writer();
        reader              = new Reader();
        damage_events       = new List<DamageEvent>();
        interaction_events  = new List<InteractionEvent>();

        if (recover_session)
        {
            ReadDamageEvents();
            ReadInteractionEvents();
        }
    }

    void OnApplicationQuit()
    {
        if (record_session)
        {
            WriteDamageEvents();
            WriteInteractionEvents();
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
        writer.Write(damage_events_filename, serialized_data);

    }

    void ReadDamageEvents()
    {
        string[][] data = reader.Read(damage_events_filename);
        foreach (string[] row in data)
        {
            DamageEvent n_event = new DamageEvent();
            n_event.FromSerialized(row);
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

        foreach (DamageEvent row in damage_events)
            data.Add(row.GetSerialized());

        string[,] serialized_data = CreateRectangularArray<string>(data);
        writer.Write(interaction_events_filename, serialized_data);
    }

    void ReadInteractionEvents()
    {
        string[][] data = reader.Read(interaction_events_filename);
        foreach (string[] row in data)
        {
            InteractionEvent n_event = new InteractionEvent();
            n_event.FromSerialized(row);
            interaction_events.Add(n_event);
        }
    }

    public void CreateDeathEvent(GameObject player)
    {
        DamageEvent new_event = new DamageEvent((uint)damage_events.Count, System.DateTime.Now, player.transform.position, "Death");
        damage_events.Add(new_event);
    }

    public void CreateDamageEvent(GameObject player)
    {
        DamageEvent new_event = new DamageEvent((uint)damage_events.Count, System.DateTime.Now, player.transform.position, "Damage");
        damage_events.Add(new_event);
    }

    public void CreateKillEvent(GameObject what)
    {
        DamageEvent new_event = new DamageEvent((uint)damage_events.Count, System.DateTime.Now, what.transform.position, "Kill", what.name);
        damage_events.Add(new_event);
    }

    public void CreateInteractionEvent(GameObject what)
    {
        InteractionEvent new_event = new InteractionEvent((uint)interaction_events.Count, System.DateTime.Now, what.transform.position, what.name);
        interaction_events.Add(new_event);
    }


}