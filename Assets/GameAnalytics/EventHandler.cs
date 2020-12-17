using System;
using System.Collections.Generic;
using UnityEngine;


public class EventHandler : MonoBehaviour
{
    List<DamageEvent> damage_events;
    List<InteractionEvent> interaction_events;
    public bool record_session = false;
    Writer writer;

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
        writer = new Writer();
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
        writer.Write("DamageEvents", serialized_data);

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
        writer.Write("InteractionEvents", serialized_data);
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