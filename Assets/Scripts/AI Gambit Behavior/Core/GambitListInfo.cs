using System;
using System.Collections.Generic;
using System.Linq;
using System.Monads;
using UniRx;
using UnityEngine;
using System.Collections;

namespace AI.Gambits {

	[CreateAssetMenu(menuName = "Create/Gambit List Info")]
	public class GambitListInfo : ScriptableObject {

        [SerializeField]
        private GambitInfo[] gambitInfos;

        public class GambitList : IInputSource, IDisposable {

            public IObservable<Vector3> moveInput { get; private set; }

            public IReadOnlyReactiveProperty<object> targets { get; private set; }

            private IList<Gambit> _gambits;

            private readonly GambitListInfo _gambitListInfo;

            private IDisposable _disposable;
            private Character _character;

            public GambitList( GambitListInfo gambitListInfo ) {

                this._gambitListInfo = gambitListInfo;

                moveInput = new Subject<Vector3>();
                targets = new ReactiveProperty<object>();
            }

            public void Initialize( Character character ) {

                this._character = character;
                this._gambits = _gambitListInfo.gambitInfos.Select( _ => _.GetGambit( character ) ).ToArray();

                _disposable = Observable.EveryUpdate().Subscribe( Tick );
            }

            private void Tick( long ticks ) {
                
                if (_character != null) {
                    
                    _gambits.FirstOrDefault( thatCan => thatCan.Execute() );
                } else {
                    
                    Dispose();
                }
            }

            public void Dispose() {

                _disposable.Dispose();
            }

        }

        public GambitList GetGambitList() {

            return new GambitList( this );
        }

    }

}