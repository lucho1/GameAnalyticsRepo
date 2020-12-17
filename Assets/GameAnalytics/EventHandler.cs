using System;
using System.Collections.Generic;
using UnityEngine;


public class EventHandler : MonoBehaviour
{
    List<DamageEvent> damage_events;
    List<InteractionEvent> interaction_events;

    public string damageEventsFilename = "DamageEvents";
    public string interactionEventsFilename = "InteractionEvents";
    public bool recordSession = false;
    public bool recoverSession = true;


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

        if (recoverSession)
        {
            ReadDamageEvents();
            ReadInteractionEvents();
        }
    }

    void OnApplicationQuit()
    {
        if (recordSession)
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