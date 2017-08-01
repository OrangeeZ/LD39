using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Packages.EventSystem;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIScreen
{
    [SerializeField]
    private Slider _healtBar;

    [SerializeField]
    private Slider _powersBar;

    private Character _targetCharacter;

    void Update()
    {
        if (GameplayController.Instance == null)
        {
            return;
        }

        if (_targetCharacter == null)
        {
            _targetCharacter = Character.Instances.FirstOrDefault(_ => _.IsPlayerCharacter);
        }

        if (_targetCharacter != null)
        {
            _healtBar.normalizedValue = _targetCharacter.Health.Value / _targetCharacter.Status.Info.MaxHealth;
        }
        
        _powersBar.value = GameplayController.Instance.CurrentPowerNormalized;
    }
}