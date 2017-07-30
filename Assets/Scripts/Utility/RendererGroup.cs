using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererGroup : MonoBehaviour
{
    [SerializeField]
    private Renderer[] _renderers;

    [SerializeField]
    private string _groupId;

    void Reset()
    {
        _renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
    }

    public bool CompareId(string id)
    {
        return string.Compare(id, _groupId, StringComparison.InvariantCultureIgnoreCase) == 0;
    }
    
    public void SetActive(bool isActive)
    {
        foreach (var each in _renderers)
        {
            each.enabled = isActive;
        }
    }
}