﻿using System;
using System.Linq;
using Packages.EventSystem;
using UniRx;
using UnityEngine;

public class PlayerCharacterSpawner : MonoBehaviour
{
    public class Spawned : IEventBase
    {
        public Character Character;
    }

    public CharacterInfo characterInfo;
    public CharacterStatusInfo CharacterStatusInfo;

    public ItemInfo[] startingItems;

    public WeaponInfo startingWeapon;
    public WeaponInfo StartingSecondaryWeapon;

    public CharacterStatusEffectInfo startingStatusEffect;

    public CameraBehaviour cameraBehaviour;

    public Character character;

    [SerializeField]
    private bool _initializeOnStart;
    
    void Start()
    {
        if (_initializeOnStart)
        {
            Initialize();
        }
    }

    public void Initialize()
    {
        character = characterInfo.GetCharacter(startingPosition: transform.position, replacementStatusInfo: CharacterStatusInfo);

        foreach (var each in startingItems.Select(_ => _.GetItem()))
        {
            character.Inventory.AddItem(each);
        }

        var weaponInfo = startingWeapon;
//		if ( CharacterStatusInfo != null ) {
//
//			startingWeapon = startingWeapon ?? CharacterStatusInfo.BaseWeapon;
//		}

        if (weaponInfo != null)
        {
            var weapon = weaponInfo.GetItem();

            character.Inventory.AddItem(weapon);
            weapon.Apply();
        }
        
        if (StartingSecondaryWeapon != null)
        {
            var weapon = StartingSecondaryWeapon.GetItem();

            character.Inventory.AddItem(weapon);
            weapon.Apply();
        }

        if (cameraBehaviour != null)
        {
            var cameraBehaviourInstance = Instantiate(cameraBehaviour);
            cameraBehaviourInstance.transform.position = transform.position;
            cameraBehaviourInstance.SetTarget(character.Pawn);
        }

        if (startingStatusEffect != null)
        {
            startingStatusEffect.Add(character);
        }

        character.IsPlayerCharacter = true;

        EventSystem.RaiseEvent(new Spawned {Character = character});
    }
}