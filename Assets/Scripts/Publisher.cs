using System;
using System.Collections.Generic;
using UnityEngine;

// Modified from exercise 3
public class Publisher : IPublisher
{
    private List<Action> collection;

    public Publisher()
    {
        this.collection = new List<Action>();
    }

    public void Subscribe(Action notifier)
    {
        this.collection.Add(notifier);   
    }

    public void Unsubscribe(Action notifier)
    {
        this.collection.Remove(notifier);
    }

    public void Notify()
    {
        foreach (Action notifier in this.collection)
        {
            notifier();
        }
    }
}