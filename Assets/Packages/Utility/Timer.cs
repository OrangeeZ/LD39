using UnityEngine;
using System.Collections;

public class StepTimer
{
    public float Value
    {
        get { return timerValue; }
    }

    public float ValueNormalized
    {
        get { return timerValue / duration; }
    }

    private float timerValue;

    private readonly float duration;

    public StepTimer(float duration)
    {
        this.duration = duration;
    }

    public void Step()
    {
        Step(Time.deltaTime);
    }

    public void Step(float deltaTime)
    {
        timerValue = (timerValue + deltaTime).Clamped(0, duration);
    }
}

public class AutoTimer
{
    public float Value => _useUnscaledTime ? Time.unscaledTime - _startTime : Time.time - _startTime;

    public float ValueNormalized => (Value / _duration).Clamped(0, 1);

    private readonly bool _useUnscaledTime = false;

    private readonly float _duration;
    private float _startTime;

    public AutoTimer(float duration, bool useUnscaledTime = false)
    {
        _duration = duration;
        _useUnscaledTime = useUnscaledTime;

        Reset();
    }

    public void Reset()
    {
        _startTime = _useUnscaledTime ? Time.unscaledTime : Time.time;
    }
}