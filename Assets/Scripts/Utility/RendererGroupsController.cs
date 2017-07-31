using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class RendererGroupsController : MonoBehaviour
{
    [SerializeField]
    private RendererGroup[] _rendererGroups;

    [SerializeField]
    private string _startingRendererGroupId;

    private RendererGroup _lastRendererGroup;

    void Reset()
    {
        _rendererGroups = GetComponentsInChildren<RendererGroup>(includeInactive: true);
    }

    void Start()
    {
        foreach (var each in _rendererGroups)
        {
            each.SetActive(false);
        }

        SetRendererGroupActive(_startingRendererGroupId);
    }

    public void SetRendererGroupActive(string id)
    {
        var targetRendererGroup = _rendererGroups.FirstOrDefault(_ => _.CompareId(id));

        if (targetRendererGroup != null)
        {
            _lastRendererGroup?.SetActive(false);
            targetRendererGroup.SetActive(true);
            _lastRendererGroup = targetRendererGroup;
        }
    }

    public void SetAnimationDirection(float direction)
    {
        if (_lastRendererGroup != null)
        {
            var scale = Vector3.one;
            scale.x *= direction;

            _lastRendererGroup.transform.localScale = scale;
        }
    }
}