using UniRx;

namespace Packages.EventSystem {

	public interface IEventBase { }

	public class EventSystem {

		public static Subject<IEventBase> Events = new Subject<IEventBase>();

	    public static void Reset() {
	        
            Events = new Subject<IEventBase>();
	    }

		public static void RaiseEvent( IEventBase raisedEvent ) {
			
			Events.OnNext( raisedEvent );
		}
	}
}
