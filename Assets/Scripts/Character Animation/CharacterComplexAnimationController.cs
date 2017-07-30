using System.Linq;
using UnityEngine;
using System.Collections;

public class CharacterComplexAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator[] _animators;

    private string _lastState = null;

    void Reset()
    {
        this.GetComponentsInChildren(out _animators, includeInactive: true);
    }

    public void SetBool(string name, bool value)
    {
        if (name.IsNullOrEmpty())
        {
            return;
        }

        if (!_lastState.IsNullOrEmpty())
        {
            _animators.SetBool(_lastState, false);
        }

        _animators.SetBool(name, value);

        _lastState = name;
    }
}