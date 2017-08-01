using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplayBiteTimer : MonoBehaviour
{
    public static DisplayBiteTimer Instance { get; private set; }

    [SerializeField]
    private BiteTimer _prefab;
    
    private Dictionary<Character, BiteTimer> _activeTimers = new Dictionary<Character, BiteTimer>();
    
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        var timersToRemove = _activeTimers.Where(_ => _.Key.Health.Value == 0).ToArray();
        foreach (var each in timersToRemove)
        {
            _activeTimers.Remove(each.Key);
            Destroy(each.Value.gameObject);
        }
    }

    public void Display(Character target, float value)
    {
        if (target.Health.Value == 0)
        {
            return;
        }
        
        if (!_activeTimers.ContainsKey(target))
        {
            _activeTimers[target] = Instantiate(_prefab);
        }

        _activeTimers[target].GetComponent<UIElementWorldAnchor>().Target = target.Pawn.transform;
    }
}