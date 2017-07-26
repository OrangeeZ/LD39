using System.Collections;
using System.Linq;
using Packages.EventSystem;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Game_UI
{
    public class RoomsStatusView : MonoBehaviour
    {
        [SerializeField]
        private Image _leftBar;
        [SerializeField]
        private Image _rightBar;
        [SerializeField]
        private Text _progressAmount;

        private float  _charactersCount;
        private float _currentCharacterCount;

        // Use this for initialization
        private IEnumerator Start()
        {
            yield return null;
            EventSystem.Events.SubscribeOfType<Room.CharacterDied>(OnCharacterDieInRoom);
            var rooms = Room.GetRooms();
            foreach (var room in rooms)
            {
                var type = room.GetRoomType();
                if (type != Room.RoomType.AlienMotherRoom)
                {
                    _charactersCount += room.GetCharacters().Count;
                }
            }
            _currentCharacterCount = _charactersCount;
        }


        private void OnCharacterDieInRoom(Room.CharacterDied characterDied)
        {
            var type = characterDied.Room.GetRoomType();
            if (type == Room.RoomType.AlienMotherRoom)
            {
                return;
            }
                _currentCharacterCount--;
            if (_currentCharacterCount <= 0)
            {
                _leftBar.fillAmount = 0;
                _rightBar.fillAmount = 0;
                _progressAmount.text = "0";
                return;
            }
            var fillAmount = _currentCharacterCount/ _charactersCount;
            _progressAmount.text = string.Format("{0}",Mathf.RoundToInt(fillAmount*100));
            _leftBar.fillAmount = fillAmount;
            _rightBar.fillAmount = fillAmount;
        }
    }
}
