using System.Collections.Generic;
using System.Linq;
using Packages.EventSystem;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Game_UI
{
    public class RoomCrewStatus : MonoBehaviour
    {
        [SerializeField]
        private Image RoomDisabled;
        [SerializeField]
        private Button Background;
        [SerializeField]
        private Room.RoomType _roomType;

        private List<CrewView> _crewViews = new List<CrewView>(); 

        // Use this for initialization
        private void Start ()
        {
            gameObject.GetComponentsInChildren<CrewView>(true, _crewViews);
            EventSystem.Events.SubscribeOfType<Room.EveryoneDied>(OnEveryoneDieInRoom);
            EventSystem.Events.SubscribeOfType<Room.CharacterDied>(OnCharacterDieInRoom);
        }
    
        private void OnEveryoneDieInRoom(Room.EveryoneDied everyoneDiedEvent)
        {
            var room = everyoneDiedEvent.Room;
            if (room.GetRoomType() != _roomType) return;
            Background.interactable = false;
            RoomDisabled.gameObject.SetActive(true);
        }


        private void OnCharacterDieInRoom(Room.CharacterDied everyoneDiedEvent)
        {
            var room = everyoneDiedEvent.Room;
            if (room.GetRoomType() != _roomType) return;
            var aliveCharacter = _crewViews.FirstOrDefault(x => !x.IsDead);
            if (aliveCharacter != null)
            {
                aliveCharacter.MarkAsDead(true);
            }
        }
    }
}
