using System;
using UniRx;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    public Subject<string> AnimationEvents = new Subject<string>();

    public void NotifyEventTriggered(string eventName)
    {
        Debug.Log($"Animation event: {eventName}");
        AnimationEvents.OnNext(eventName);
    }
}