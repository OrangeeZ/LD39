using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeController : MonoBehaviour
{
    public float TimeScaleRate => Time.timeScale;
    
    [SerializeField]
    private GlobalGameInfo _gameInfo;

    void Start()
    {
        Time.timeScale = _gameInfo.StartingTimeScale;
    }
    
    void Update()
    {
        Time.timeScale += _gameInfo.TimeAccelerationRate * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp01(Time.timeScale);
    }
}
