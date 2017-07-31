using UniRx;
using UnityEngine;
using System.Collections;

public class CharacterPawnBase : AObject
{
    public CharacterComplexAnimationController _animationController;

    public CharacterComplexAnimationController _weaponAnimationController;

    public CharacterComplexAnimationController animatedView { get; private set; }

    public Character Character { get; private set; }

    protected float DeltaTime => Character != null && Character.UsesUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;

    protected float speed;

    [SerializeField]
    private SphereSensor sensor;
    
    protected virtual void Start()
    {
        animatedView = _animationController;
    }

    public SphereSensor GetSphereSensor()
    {
        return sensor;
    }

    public void SetCharacter(Character character)
    {
        Character = character;

        if (_animationController != null)
        {
            _animationController.UsesUnscaledDeltaTime = character.UsesUnscaledDeltaTime;            
        }

        if (_weaponAnimationController)
        {
            _weaponAnimationController.UsesUnscaledDeltaTime = character.UsesUnscaledDeltaTime;            
        }
        
        sensor.SetCharacter(character);
    }

    public virtual void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public virtual void MoveHorizontal(Vector3 direction)
    {
        transform.position += speed * direction * DeltaTime;
    }

    public virtual void SetDestination(Vector3 destination)
    {
        //navMeshAgent.destination = destination;
    }

    public virtual float GetDistanceToDestination()
    {
        return float.NaN;
    }

    public virtual Vector3 GetDirectionTo(CharacterPawnBase otherPawn)
    {
        return Vector3.forward;
    }

    public virtual void MakeDead()
    {
    }
}