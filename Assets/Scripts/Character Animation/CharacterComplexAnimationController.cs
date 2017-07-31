using UnityEngine;

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
    
    public void SetBoolInclusive(string name, bool value)
    {
        if (name.IsNullOrEmpty())
        {
            return;
        }

        _animators.SetBool(name, value);
    }
    
    public void SetFloat(string name, float value)
    {
        if (name.IsNullOrEmpty())
        {
            return;
        }

        foreach (var each in _animators)
        {
            if (!each.isActiveAndEnabled)
            {
                continue;
            }
            
            each.SetFloat(name, value);
        }
    }
}