using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] _rigidbodies;

    [SerializeField]
    private GameObject[] _additionalGameObjects;

    void Reset()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>(includeInactive: true);
    }

    void Awake()
    {
        SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        var animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.enabled = !isActive;
        }

        foreach (var each in _rigidbodies)
        {
            each.detectCollisions = isActive;
            each.isKinematic = !isActive;
            each.useGravity = isActive;
            each.ResetInertiaTensor();
        }

        foreach (var each in _additionalGameObjects)
        {
            each.SetActive(!isActive);
        }
    }
}