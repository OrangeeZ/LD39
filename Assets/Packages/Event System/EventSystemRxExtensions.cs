using System;
using Packages.EventSystem;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

namespace UniRx {

	public static partial class ObservableExtensions {

		public static IDisposable SubscribeOfType<T>( this IObservable<IEventBase> self, Action<T> listener ) where T : IEventBase {

			return self.OfType<IEventBase, T>().Subscribe( listener );
		}

		public static IDisposable SubscribeOfType<T>( this IObservable<IEventBase> self, Action<T> listener, GameObject cancellationToken ) where T : IEventBase {

			var result = self.SubscribeOfType( listener );

			cancellationToken.OnDestroyAsObservable().Subscribe( _ => result.Dispose() );

			return result;
		}

        public static IDisposable SubscribeOfType<T>(this ISubject<IEventBase> self, Action<T> listener) where T : IEventBase
        {

            return self.OfType<IEventBase, T>().Subscribe(listener);
        }

        public static IDisposable SubscribeOfType<T>(this ISubject<IEventBase> self, Action<T> listener, GameObject cancellationToken) where T : IEventBase
        {

            var result = self.SubscribeOfType(listener);

            cancellationToken.OnDestroyAsObservable().Subscribe(_ => result.Dispose());

            return result;
        }
    }
}
