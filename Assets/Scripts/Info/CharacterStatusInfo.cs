using UnityEngine;
using System.Collections;
using Expressions;
using UniRx;

[CreateAssetMenu(menuName = "Create/Status Info")]
public class CharacterStatusInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public float MaxHealth;

    [RemoteProperty]
    public float MoveSpeed;

    [RemoteProperty]
    public float JumpSpeed;

    [RemoteProperty]
    public float JumpHeight;

    [RemoteProperty]
    public float RollSpeed;

    [RemoteProperty]
    public float RollAcceleration;

    [RemoteProperty]
    public float RollDamage;
    
    [Space]
    public AudioClip[] IdleSounds;
    public AudioClip[] DeathSounds;

    public virtual CharacterStatus GetInstance()
    {
        return new CharacterStatus(this);
    }

    public virtual void Configure(csv.Values values)
    {
    }
}