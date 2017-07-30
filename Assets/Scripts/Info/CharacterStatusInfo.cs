using UnityEngine;

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
    public float RollDeceleration;

    [RemoteProperty]
    public float RollDamage;
       
    [RemoteProperty]
    public CharacterPawn PawnPrefab;

    [RemoteProperty]
    public float BiteStateHealthThreshold;

    [RemoteProperty]
    public float BiteStateDuration;

    [RemoteProperty]
    public float EatStateDuration;

    [RemoteProperty]
    public bool UsesUnscaledDeltaTime;

    [RemoteProperty]
    public float PowerPerEat;
    
    [Space]
    [RemoteProperty]
    public float AggroRadius;
    
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