using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Packages.EventSystem;
using UniRx;

public class Room : MonoBehaviour {

	public enum RoomType {

		Default,
		MedicalBay,
		Reactor,
		Workshop,
		ControlRoom,
		SecurityRoom,
		AlienMotherRoom

	}

	public struct EveryoneDied : IEventBase {

		public Room Room;

	}

	public struct CharacterDied : IEventBase {

		public Room Room;

	}

	[SerializeField]
	private RoomType _roomType;

	[SerializeField]
	private Bounds _bounds;

	[SerializeField]
	private EnemySpawner[] _npcSpawners;

	[SerializeField]
	private EnemySpawner[] _enemySpawners;

	[SerializeField]
	private Transform[] _ventilationHatches;

	[SerializeField]
	private EnvironmentObjectSpot[] _objectSpots;

	private static List<Room> _instances = new List<Room>();

	private List<Character> _charactersInRoom = new List<Character>();

	private void Awake() {

		_instances.Add( this );
	}

	private void Start() {

		EventSystem.Events.SubscribeOfType<Character.Died>( OnCharacterDie );
	}

	private void OnDestroy() {

		_instances.Remove( this );
	}

	public void Initialize() {

		foreach ( var each in _npcSpawners ) {

			each.Initialize();

			_charactersInRoom.Add( each.GetLastSpawnedCharacter() );
		}

		foreach ( var each in _enemySpawners ) {

			each.Initialize();
		}
	}

	public List<Character> GetRoomCharacters( RoomType roomType ) {
		var room = _instances.FirstOrDefault( x => x.GetRoomType() == roomType );
		return room == null ? null : room._charactersInRoom;
	}

	public static List<Room> GetRooms() {
		return _instances;
	}

	public static void InitializeAll() {

		foreach ( var each in _instances ) {

			each.Initialize();
		}
	}

	public static Room FindRoomForPosition( Vector3 position ) {

		return _instances.FirstOrDefault( _ => _.Contains( position ) );
	}

	public static Room RandomRoomExcept( Room roomToAvoid ) {

		return _instances.Where( _ => _ != roomToAvoid ).RandomElement();
	}

	public bool Contains( Vector3 position ) {

		return _bounds.Contains( transform.worldToLocalMatrix.MultiplyPoint3x4( position ) );
	}

	public Vector3 FindClosestVentilationHatchPosition( Vector3 position ) {

		return _ventilationHatches.MinBy( _ => Vector3.SqrMagnitude( position - _.position ) ).position;
	}

	public EnvironmentObjectSpot FindRandomObjectSpot( Vector3 position ) {

		return _objectSpots.Where( _ => ( _.GetState() == EnvironmentObjectSpot.State.Empty || _.GetState() == EnvironmentObjectSpot.State.Destroyed ) && !_.IsReserved ).RandomElement();
	}

	public List<Character> GetCharacters() {
		return _charactersInRoom;
	}

	public RoomType GetRoomType() {

		return _roomType;
	}

	public Vector3 GetRandomPoint() {

		var randomX = 1f.Random() * 2f - 1;
		var randomZ = 1f.Random() * 2f - 1;

		var randomExtents = Vector3.Scale( _bounds.extents, new Vector3( randomX.Clamped( -0.8f, 0.8f ), 0, randomZ.Clamped( -0.8f, 0.8f ) ) );
		return transform.localToWorldMatrix.MultiplyPoint3x4( randomExtents + _bounds.center );
	}

	public static Character GetRandomNpc( Room roomToSkip ) {

		var allNpcs = _instances.Where( _ => _ != roomToSkip ).SelectMany( _ => _._charactersInRoom );

		return allNpcs.RandomElement();
	}

	private void OnCharacterDie( Character.Died diedEvent ) {

		if ( _charactersInRoom.Remove( diedEvent.Character ) ) {
			EventSystem.RaiseEvent( new CharacterDied() {Room = this} );
			if ( _charactersInRoom.IsEmpty() ) {

				EventSystem.RaiseEvent( new EveryoneDied {Room = this} );
			}
		}
	}

	private void OnDrawGizmos() {

		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube( _bounds.center, _bounds.size );
	}

	[ContextMenu( "Hook spots" )]
	private void HookSpots() {

		_ventilationHatches = transform.OfType<Transform>().Where( _ => _.name.ToLower().Contains( "hatch" ) ).ToArray();
		_objectSpots = GetComponentsInChildren<EnvironmentObjectSpot>( includeInactive: true );
		_npcSpawners = GetComponentsInChildren<EnemySpawner>( includeInactive: true ).Where( _ => _.name.ToLower().Contains( "npc" ) ).ToArray();
		_enemySpawners = GetComponentsInChildren<EnemySpawner>( includeInactive: true ).Where( _ => !_.name.ToLower().Contains( "npc" ) ).ToArray();
	}

}