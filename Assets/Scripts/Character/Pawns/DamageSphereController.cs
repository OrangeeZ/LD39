using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DamageSphereController : MonoBehaviour
{
    private Character _ownerCharacter;

    void Awake()
    {
        SetActive(false);
    }

    public void SetOwnerCharacter(Character ownerCharacter)
    {
        _ownerCharacter = ownerCharacter;
    }
    
    public void SetActive(bool isActive)
    {
        GetComponent<Collider>().enabled = isActive;
    }

    void OnTriggerEnter(Collider other)
    {
        var otherPawn = other.transform.root.gameObject.GetComponent<CharacterPawn>();
        var otherCharacter = otherPawn?.character;

        if (otherCharacter != _ownerCharacter && otherCharacter?.TeamId != _ownerCharacter.TeamId)
        {
            otherCharacter?.Damage(_ownerCharacter.Status.Info.RollDamage);
        }
    }
}