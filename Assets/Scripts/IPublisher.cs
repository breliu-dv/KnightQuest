using System;
using UnityEngine;

// Modified from exercise 3
public interface IPublisher
{
    void Unsubscribe(Action notifier);

    void Subscribe(Action notifier);

    void Notify();
}
