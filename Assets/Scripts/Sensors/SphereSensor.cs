using System;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class SphereSensor : MonoBehaviour, IObservable<CharacterPawnBase>
{
    public IList<CharacterPawnBase> NearbyCharacters => _nearbyCharacters;

    [SerializeField]
    private SphereCollider _collider;

    private readonly Subject<CharacterPawnBase> _pawnSubject = new Subject<CharacterPawnBase>();

    private readonly List<CharacterPawnBase> _nearbyCharacters = new List<CharacterPawnBase>();

    void OnEnable()
    {
        if (_collider != null)
        {
            _collider.enabled = true;
        }
    }

    void OnDisable()
    {
        if (_collider != null)
        {
            _collider.enabled = false;
        }

        _nearbyCharacters.Clear();
    }

    public void SetRadius(float radius)
    {
        _collider.radius = radius;
    }

    public IDisposable Subscribe(IObserver<CharacterPawnBase> observer)
    {
        return _pawnSubject.Subscribe(observer);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherPawn = other.transform.root.gameObject.GetComponent<CharacterPawnBase>();

        if (otherPawn != null)
        {
            _pawnSubject.OnNext(otherPawn);
            _nearbyCharacters.Add(otherPawn);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var otherPawn = other.transform.root.gameObject.GetComponent<CharacterPawnBase>();

        if (otherPawn != null)
        {
            _nearbyCharacters.Remove(otherPawn);
        }
    }
}