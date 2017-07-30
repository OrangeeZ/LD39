using System.Linq;
using UnityEngine;
using System.Collections;

public class CharacterComplexAnimationController : MonoBehaviour
{
    public bool UsesUnscaledDeltaTime
    {
        set
        {
            foreach (var each in _animators)
            {
                each.updateMode = value ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
            }
        }
    }

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