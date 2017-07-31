using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Create/Weapon/Behaviours/Default")]
public class DefaultRangedBehaviourInfo : RangedWeaponBehaviourInfo
{
    private class DefaultRangedBehaviour : RangedWeaponBehaviour
    {
        public int AmmoInClip { get; private set; }

        public override bool IsReloading
        {
            get
            {
                if (_isReloading && GetTimestamp() > _nextAttackTime)
                {
                    _isReloading = false;
                }

                return _isReloading;
            }

            protected set { _isReloading = value; }
        }

        private bool _isReloading;
        private float _nextAttackTime;
        private RangedWeaponInfo.RangedWeapon _ownerWeapon;
        private bool _useUnscaledTime;

        public override void Initialize(IInventory ownerInventory, RangedWeaponInfo.RangedWeapon ownerWeapon, bool useUnscaledTime)
        {
            _ownerWeapon = ownerWeapon;
            _useUnscaledTime = useUnscaledTime;

            AmmoInClip = _ownerWeapon.ClipSize;
        }

        public override bool TryShoot()
        {
            if (IsReloading)
            {
                return false;
            }
            
            AmmoInClip--;

            if (AmmoInClip == 0)
            {
                AmmoInClip = _ownerWeapon.ClipSize;
                
                _nextAttackTime = GetTimestamp() + _ownerWeapon.ReloadDuration;

                IsReloading = true;

                return false;
            }

            _nextAttackTime = GetTimestamp() + 1f / _ownerWeapon.BaseAttackSpeed;

            return true;
        }

        private float GetTimestamp()
        {
            return _useUnscaledTime ? Time.realtimeSinceStartup : Time.timeSinceLevelLoad;
        }
    }

    public override RangedWeaponBehaviour GetBehaviour()
    {
        return new DefaultRangedBehaviour();
    }
}