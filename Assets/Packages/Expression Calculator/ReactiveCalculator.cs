using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Expressions {

    [Serializable]
    public class ReactiveCalculator : IObservable<double> {

        public double this[ string key ] {
            set {

                var lowercaseKey = key.ToLower();

                var oldValue = _calculator.GetVariable( lowercaseKey );

                if ( oldValue != value ) {

                    _calculator.SetVariable( lowercaseKey, value );

                    Recalculate();
                }
            }
        }

        public string Expression {
            get { return _expression; }
            set {

                if ( _expression != value ) {

                    _expression = value;

                    Recalculate();
                }
            }
        }

        public bool IsValid {
            get { return _isValid; }
            private set { _isValid = value; }
        }

        public DoubleReactiveProperty Result = new DoubleReactiveProperty( 0 );

        private readonly Calculator _calculator = new Calculator();

        private Dictionary<string, IDisposable> _subscriptions = new Dictionary<string, IDisposable>();

        [SerializeField, HideInInspector]
        private string _expression;

        [SerializeField, HideInInspector]
        private bool _isValid;

        public ReactiveCalculator( IReactiveProperty<string> expression ) {

            _subscriptions["Expression"] = expression.Subscribe( _ => Expression = _ );
        }

        ~ReactiveCalculator() {

            foreach ( var each in _subscriptions.Values ) {

                each.Dispose();
            }

            _subscriptions = null;
        }

        public void SetExpression( string newExpression ) {

            if ( _expression != newExpression ) {

                _expression = newExpression;

                Recalculate();
            }
        }

        public void SubscribeProperty( string name, IReactiveProperty<double> property ) {

            if ( _subscriptions.ContainsKey( name ) ) {

                _subscriptions[name].Dispose();
            }

            _subscriptions[name] = property.Subscribe( _ => this[name] = _ );
        }

        public void SubscribeProperty( string name, IReactiveProperty<int> property ) {

            if ( _subscriptions.ContainsKey( name ) ) {

                _subscriptions[name].Dispose();
            }

            _subscriptions[name] = property.Subscribe( _ => this[name] = _ );
        }

        public IDisposable Subscribe( IObserver<double> observer ) {

            return Result.Subscribe( observer );
        }

        private void Recalculate() {

            try {

                _calculator.Clear();
                Result.Value = _calculator.Evaluate( _expression );

                IsValid = true;
            }
            catch {

                IsValid = false;
            }
        }

    }

}