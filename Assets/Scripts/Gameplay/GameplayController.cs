using System;
using UnityEngine;
using System.Collections;
using UniRx;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance { get; private set; }

    public float CurrentPower { get; private set; }
    public float CurrentPowerNormalized => CurrentPower / _globalGameInfo.MaxPower;
    
    [SerializeField]
    private PlayerCharacterSpawner _playerSpawner;

    [SerializeField]
    private SpawnerBase[] _enemySpawners;

    [SerializeField]
    private GlobalGameInfo _globalGameInfo;

    void Awake()
    {
        Instance = this;
    }

    public IEnumerator Start()
    {
        yield return null;

        _playerSpawner.Initialize();

        foreach (var each in _enemySpawners)
        {
            each.Initialize();
        }

        CurrentPower = _globalGameInfo.StartingPower;
    }
    
    public void AddPower(float power)
    {
        CurrentPower += power;
    }

    void Update()
    {
        CurrentPower -= _globalGameInfo.PowerDecreaseSpeed * Time.unscaledDeltaTime;
        CurrentPower = CurrentPower.Clamped(0, _globalGameInfo.MaxPower);

        Time.timeScale = 1f - CurrentPower / _globalGameInfo.MaxPower;
    }

    [ContextMenu("Hook dependencies")]
    private void HookDependencies()
    {
        _playerSpawner = FindObjectOfType<PlayerCharacterSpawner>();
        _enemySpawners = FindObjectsOfType<SpawnerBase>();
    }

}