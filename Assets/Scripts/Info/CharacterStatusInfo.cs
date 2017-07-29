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

    public AudioClip[] IdleSounds;
    public AudioClip[] DeathSounds;

    [SerializeField]
    private CharacterStatus status;

    public virtual CharacterStatus GetInstance()
    {
        return new CharacterStatus(this);
    }

    public virtual void Configure(csv.Values values)
    {
    }
}