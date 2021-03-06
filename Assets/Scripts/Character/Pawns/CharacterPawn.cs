﻿using UniRx;
using UnityEngine;

public class CharacterPawn : CharacterPawnBase
{
    public bool CanFollowDestination;
    
    [SerializeField]
    private Vector3 _gunOffset = Vector3.right;

    [SerializeField]
    private float _weight = 1f;

    [SerializeField]
    private float _isGroundedChangeDelay = 0.2f;

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private CharacterControllerSizeController _characterControllerSizeController;

    [SerializeField]
    private DamageSphereController _damageSphereController;

    [SerializeField]
    private Transform _gunTransform;
    
    private Vector3? _destination;

    private Transform _turretTarget;

    [SerializeField]
    private CharacterSpriteAnimationController _spriteAnimationController;

    [SerializeField]
    private RotatingTransform _rollSpriteRoot;

    [SerializeField]
    private RagdollController _ragdollController;

    private float _lastGroundedTime = 0f;

    private float _animatorDirection = 1f;

    protected virtual void Update()
    {
        if (_characterController != null && _characterController.enabled)    
        {
            _characterController.Move(Vector3.down * DeltaTime * _weight);

            if (_characterController.isGrounded)
            {
                _lastGroundedTime = Time.time;
            }
        }
    }

    public Vector3 GetWeaponPosition()
    {
        return _gunTransform.position + _gunOffset * _animatorDirection;
    }

    public override void SetHeightNormalized(float newNormalizedHeight)
    {
        _characterControllerSizeController?.SetHeightNormalized(newNormalizedHeight);
    }

    public override void MoveHorizontal(Vector3 direction)
    {
        var directionDelta = direction * speed * DeltaTime;

        if (_characterController == null)
        {
            position += directionDelta;
        }
        else
        {
            _characterController.Move(directionDelta);
        }

        _animatorDirection = Mathf.Sign(direction.x);

        if (_rollSpriteRoot != null)
        {
            _rollSpriteRoot.RotateByLinearMotion(-directionDelta.magnitude);
        }

        UpdateSpriteAnimationDirection(direction);
    }

    public void MoveVertical(ref float impulse, float deltaTime)
    {
        impulse += Physics.gravity.y * _weight * deltaTime;

        impulse = impulse < 0 && IsGrounded() ? 0 : impulse;

        var delta = impulse * Vector3.up;

        if (_characterController == null)
        {
            position += delta * deltaTime;
        }
        else
        {
            _characterController.Move(delta * deltaTime);
        }
    }

    public override void SetDestination(Vector3 destination)
    {
        _destination = destination;
    }

    public override float GetDistanceToDestination()
    {
        return _destination.HasValue ? Vector3.Distance(position, _destination.Value) : float.NaN;
    }

    public override Vector3 GetDirectionTo(CharacterPawnBase otherPawn)
    {
        return (otherPawn.position - position).normalized;
    }

    public virtual void ClearDestination()
    {
        _destination = null;
    }

    public void SetColor(Color baseColor)
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var each in renderers)
        {
            each.material.SetColor("_Color", baseColor);
        }
    }

    public void SetActive(bool isActive)
    {
        enabled = isActive;
        gameObject.SetActive(isActive);
    }

    public override void MakeDead()
    {
        GetSphereSensor().enabled = false;
        GetComponent<Collider>().enabled = false;
        _ragdollController?.SetActive(true);
    }

    public override Vector3 GetCenter()
    {
        if (_characterController != null)
        {
            return (_characterController.bounds.min);            
        }
        else
        {
            return base.GetCenter();
        }
    }

    public void UpdateSpriteAnimationDirection(Vector3 direction)
    {
        if (_animationController != null)
        {
            _animatorDirection = Mathf.Sign(direction.x);
            _animationController.transform.localRotation = Quaternion.AngleAxis(_animatorDirection < 0 ? 180f : 0f, Vector3.up);

            GetComponent<RendererGroupsController>()?.SetAnimationDirection(_animatorDirection);
            GetComponent<ParticleSystemsController>()?.SetAnimationDirection(_animatorDirection);
        }
        
        if (_spriteAnimationController == null)
        {
            return;
        }

        var directionX = (int) Mathf.Clamp(-direction.x * 100, -1, 1);
        var directionY = (int) Mathf.Clamp(-direction.z * 100, -1, 1);

        _spriteAnimationController.UpdateDirection(directionX, directionY);
    }

    public bool IsGrounded()
    {
        return Time.time - _lastGroundedTime < _isGroundedChangeDelay;
    }

    public void SetDamageSphereActive(bool isActive)
    {
        _damageSphereController.SetOwnerCharacter(Character);
        _damageSphereController.SetActive(isActive);
    }

//    void OnControllerColliderHit(ControllerColliderHit hit)
//    {
//        Debug.Log(hit.normal);
//    }
}