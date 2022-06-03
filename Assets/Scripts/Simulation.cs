using System;
using System.Collections.Generic;
using UnityEngine;

// The Simulation class implements the discrete event simulator pattern.
// Events are pooled, with a default capacity of 4 instances.
public static partial class Simulation
{
    static Dictionary<System.Type, Stack<Event>> eventPools = new Dictionary<System.Type, Stack<Event>>();

    // Create a new event of type T and return it, but do not schedule it.
    static public T New<T>() where T : Event, new()
    {
        Stack<Event> pool;
        
        if (!eventPools.TryGetValue(typeof(T), out pool))
        {
            pool = new Stack<Event>(4);
            pool.Push(new T());
            eventPools[typeof(T)] = pool;
        }

        if (pool.Count > 0)
        {
            return (T)pool.Pop();
        }
        else
        {
            return new T();
        }
    }

    // Return the simulation model instance for a class.
    static public T GetModel<T>() where T : class, new()
    {
        return InstanceRegister<T>.instance;
    }

    // Set a simulation model instance for a class.
    static public void SetModel<T>(T instance) where T : class, new()
    {
        InstanceRegister<T>.instance = instance;
    }

    // Destroy the simulation model instance for a class.
    static public void DestroyModel<T>() where T : class, new()
    {
        InstanceRegister<T>.instance = null;
    }
}