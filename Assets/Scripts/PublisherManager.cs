using System;
using UnityEngine;

// modified from exercise 3
public class PublisherManager : MonoBehaviour
{
    
    public int groupCount { get; } = 1;
    private IPublisher group1Publisher;     // player death publisher
    void Awake()
    {
        this.group1Publisher = new Publisher(); 
    }

    /// <summary>
    /// Invokes callback of all subscribers to the
    /// publisher based on group id.
    /// </summary>
    /// <param name="group">The group id of the publisher to activate.</param>
    public void Trigger(int group)
    {
        switch(group)
        {
            case 1:
                this.group1Publisher.Notify();
                break;
            default:
                Debug.Log("Failed to invoke, " + group + " is not a valid group number.");
                break;
        }
    }

    /// <summary>
    /// Subscribes to a publisher based on group id. The callback is executed
    /// when the publisher of the group invokes the group id.
    /// </summary>
    /// <param name="group">The group id.</param>
    /// <param name="callback">The callback to call when a message is sent.
    /// </param>
    public void SubscribeToGroup(int group, Action callback)
    {
        switch(group)
        {
            case 1:
                this.group1Publisher.Subscribe(callback);
                break;
            default:
                Debug.Log("Failed to subscribe, " + group + " is not a valid group number.");
                break;
        }
    }

    /// <summary>
    /// Unsubscribes a callback from a group publiser.
    /// </summary>
    /// <param name="group">The group id.</param>
    /// <param name="callback">The callback to remove from the subscriber list.
    /// </param>
    public void UnsubscribeFromGroup(int group, Action callback)
    {
        switch(group)
        {
            case 1:
                this.group1Publisher.Unsubscribe(callback);
                break;
            default:
                Debug.Log("Failed to unsubscribe, " + group + " is not a valid group number.");
                break;
        }
    }
}
