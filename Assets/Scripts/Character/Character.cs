using System;
using System.Collections.Generic;
using Packages.EventSystem;
using UniRx;
using UnityEngine;

public class Character
{
    public struct Died : IEventBase
    {
        public Character Character;
    }

    public struct RecievedDamage : IEventBase
    {
        public Character Character;
        public float Damage;
    }

    public struct Speech : IEventBase
    {
        public Character Character;
        public string messageId;
    }

    public static List<Character> Instances = new List<Character>();

    public ReadOnlyReactiveProperty<float> Health { get; private set; }

    public bool IsDead { get; private set; }

    public readonly IInputSource InputSource;

    public readonly IInventory Inventory;

    public readonly CharacterPawn Pawn;

    public readonly CharacterStateController StateController;

    public readonly CharacterStateController WeaponStateController;

    public readonly int TeamId;
    public readonly CharacterInfo Info;

    public bool IsPlayerCharacter = false;

    public readonly CharacterStatus Status;

    public ItemInfo[] ItemsToDrop;
    public float dropProbability = 0.15f;
    public float speakProbability = 0.15f;

    public bool UsesUnscaledDeltaTime = false;
    public bool EnableWeaponStateController = true;

    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

    public float StatModifier = 1f;

    private bool _isInvincible;

    private ReactiveProperty<float> _health;

    public Character(CharacterPawn pawn, IInputSource inputSource, CharacterStatus status,
        CharacterStateController stateController, CharacterStateController weaponStateController, int teamId,
        CharacterInfo info)
    {
        Status = status;
        Pawn = pawn;
        InputSource = inputSource;
        StateController = stateController;
        WeaponStateController = weaponStateController;
        TeamId = teamId;
        Info = info;
        Inventory = new BasicInventory(this);
        UsesUnscaledDeltaTime = status.Info.UsesUnscaledDeltaTime;
        IsDead = false;

        _health = new ReactiveProperty<float>(Status.MaxHealth.Value);
        Health = _health.ToReadOnlyReactiveProperty();

        pawn.SetCharacter(this);

        StateController.Initialize(this);
        WeaponStateController.Initialize(this);

        var inputSourceDisposable = inputSource as IDisposable;
        if (inputSourceDisposable != null)
        {
            _compositeDisposable.Add(inputSourceDisposable);
        }

        Observable.EveryUpdate().Subscribe(OnUpdate).AddTo(_compositeDisposable);
        status.MoveSpeed.Subscribe(UpdatePawnSpeed).AddTo(_compositeDisposable);
        Health.Subscribe(OnHealthChange); //.AddTo( _compositeDisposable );

        Instances.Add(this);

        Status.ModifierCalculator.Changed += OnModifiersChange;
    }

    private void OnHealthChange(float health)
    {
        if (health <= 0)
        {
            EventSystem.RaiseEvent(new Died { Character = this });

            if (1f.Random() <= speakProbability)
            {
                EventSystem.RaiseEvent(new Speech { Character = this });
            }

            Instances.Remove(this);

            //_compositeDisposable.Dispose();

            Status.ModifierCalculator.Changed -= OnModifiersChange;
        }
    }

    public void Heal(float amount)
    {
        if (Health.Value == Status.MaxHealth.Value)
        {
            return;
        }

        var healAmount = Mathf.Min(Status.MaxHealth.Value - Health.Value, amount);

        if (healAmount > 0)
        {
            _health.Value += healAmount;
        }
    }

    public void Damage(float amount)
    {
        if (Health.Value <= 0 || _isInvincible)
        {
            return;
        }

        _health.Value -= amount;

        EventSystem.RaiseEvent(new RecievedDamage { Character = this, Damage = amount });

        if (!IsEnemy() && _health.Value <= 0)
        {
            MakeDead();
        }
    }

    public void SetInvincible(bool value)
    {
        _isInvincible = value;
    }

    public bool IsEnemy()
    {
        return TeamId != 0;
    }

    public void Dispose()
    {
        (InputSource as IDisposable)?.Dispose();

        _compositeDisposable.Dispose();

        Health.Dispose();
    }

    public void MakeDead()
    {
        IsDead = true;
    }

    private void OnUpdate(long ticks)
    {
        var deltaTime = UsesUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
        StateController.Tick(deltaTime);

        if (EnableWeaponStateController)
        {
            WeaponStateController.Tick(deltaTime);
        }
    }

    private void UpdatePawnSpeed(float speed)
    {
        Pawn.SetSpeed(speed);
    }

    private void OnModifiersChange()
    {
    }
}