using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIScreen
{
    [SerializeField]
    private Slider _powersBar;

    private GameTimeController _gameTimeController;

    void Update()
    {
        if (_gameTimeController == null)
        {
            _gameTimeController = FindObjectOfType<GameTimeController>();
        }
        
        _powersBar.value = 1f - _gameTimeController.TimeScaleRate;
    }
}