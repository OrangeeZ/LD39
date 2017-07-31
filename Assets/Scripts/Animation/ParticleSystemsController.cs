using System;
using System.Linq;
using UnityEngine;

public class ParticleSystemsController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _particleSystems;

    private float _direction;

    void Reset()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
    }

    void Start()
    {
        foreach (var each in _particleSystems)
        {
            each.Stop(withChildren: true);
        }
    }

    public void SetParticleSystemActive(string name, bool isActive)
    {
        var particleSystem = _particleSystems.FirstOrDefault(_ => string.Compare(_.name, name, StringComparison.InvariantCultureIgnoreCase) == 0);

        if (isActive)
        {
            particleSystem?.Play(withChildren: true);
        }
        else
        {
            particleSystem?.Stop(withChildren: true);
        }

        if (particleSystem != null)
        {
            var scale = Vector3.one;
            scale.x *= _direction;
            particleSystem.transform.parent.localScale = scale;
        }
    }

    public void SetAnimationDirection(float direction)
    {
        _direction = direction;
    }
}