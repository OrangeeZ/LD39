using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerSizeController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _characterController;

    private Vector3 _initialCenter;
    private float _initialHeight;

    void Start()
    {
        _initialHeight = _characterController.height;
        _initialCenter = _characterController.center;
    }

    public void SetHeightNormalized(float newHeightNormalized)
    {
        _characterController.height = _initialHeight * newHeightNormalized;
        _characterController.center = _initialCenter + Vector3.up * (newHeightNormalized - 1f);
    }
}