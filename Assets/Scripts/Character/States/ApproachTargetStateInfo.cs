using System;
using UnityEngine;
using UniRx;
using System.Collections;
using System.Monads;
using Packages.EventSystem;
using Utility;

[CreateAssetMenu(menuName = "Create/States/Approach target")]
public class ApproachTargetStateInfo : CharacterStateInfo
{
    [Header("Settings")]
    [SerializeField]
    private float _maxRange = 4f;

    [SerializeField]
    private bool _autoActivate = true;

    [SerializeField]
    private bool _clearTargetOnReach = false;

    [Serializable]
    public class State : CharacterState<ApproachTargetStateInfo>
    {
        private TargetPosition _destination;
        private bool _isFirstTimeNotice = true;
        private bool _targetIsCharacter = false;

        public State(CharacterStateInfo info) : base(info)
        {
        }

        public override void Initialize(CharacterStateController stateController)
        {
            base.Initialize(stateController);

            stateController.character.InputSource.targets.Subscribe(SetDestination);
        }

        public override bool CanBeSet()
        {
            var distanceToDestination =
                _destination.HasValue ? Vector3.Distance(character.Pawn.position, _destination.Value) : -1f;

//            Debug.Log(distanceToDestination);

            return distanceToDestination > 0
                   && distanceToDestination > GetMinDistance()
                   && distanceToDestination < typedInfo._maxRange;
        }

        public override IEnumerable GetEvaluationBlock()
        {
            if (_isFirstTimeNotice && _targetIsCharacter)
            {
//				var enemyInfo = character.Status.Info as EnemyCharacterStatusInfo;
//				var sound = enemyInfo.EnemySpottedSound.RandomElement();

//				if ( sound != null ) {
//
//					AudioSource.PlayClipAtPoint( sound, character.Pawn.position );
//				}
//
//				if ( 1f.Random() <= character.speakProbability ) {
//
//					EventSystem.RaiseEvent( new Character.Speech {Character = character, messageId = enemyInfo.SpeakLineId} );
//				}

                _isFirstTimeNotice = false;
            }

            var pawn = character.Pawn;
            pawn.SetSpeed(character.Status.MoveSpeed.Value);

            do
            {
                yield return null;

                pawn.SetDestination(_destination.Value);

                yield return null;

                //pawn.SetDestination( destination.Value );
            } while (pawn.GetDistanceToDestination() > GetMinDistance() &&
                     pawn.GetDistanceToDestination() < typedInfo._maxRange);

            //if ( pawn.GetDistanceToDestination() > typedInfo._maxRange ) {

            //	_didNoticeCharacter = false;
            //}

            if (typedInfo._clearTargetOnReach)
            {
                pawn.ClearDestination();
                _destination = null;
            }
        }

        public void SetDestination(object target)
        {
//            Debug.Log($"Set destination: {target}");
            if (target is Vector3)
            {
                _destination = (Vector3) target;
            }
            else if (target is Character)
            {
                _targetIsCharacter = true;
                var destinationTarget = (target as Character);
                _destination = destinationTarget.Pawn.transform;
            }
            else if (target is ItemView)
            {
                _destination = (target as ItemView).transform;
            }

            if (typedInfo._autoActivate)
            {
                stateController.TrySetState(this);
            }
        }

        private float GetMinDistance()
        {
            var primaryWeapon = character.Inventory.GetArmSlotItem(ArmSlotType.Primary);
            var result = primaryWeapon?.info.OfType<RangedWeaponInfo>().AttackRange ?? float.MaxValue;

            var secondaryWeapon = character.Inventory.GetArmSlotItem(ArmSlotType.Secondary);
            result = Mathf.Min(secondaryWeapon?.info.OfType<RangedWeaponInfo>().AttackRange ?? float.MaxValue, result);

            return Mathf.Max(result, 0);
        }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}