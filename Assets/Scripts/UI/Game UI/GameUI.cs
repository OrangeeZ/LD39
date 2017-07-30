using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIScreen
{
    [SerializeField]
    private Slider _powersBar;

    void Update()
    {
        if (GameplayController.Instance == null)
        {
            return;
        }
        
        _powersBar.value = GameplayController.Instance.CurrentPowerNormalized;
    }
}