using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Game_UI
{
    public class CrewView : MonoBehaviour
    {
        [SerializeField]
        private Button _statusButton;

        public bool IsDead { get; protected set; }

        public void MarkAsDead(bool isDead)
        {
            _statusButton.interactable = !isDead;
            IsDead = isDead;
        }

        //private methods
        private void Start()
        {
            _statusButton = gameObject.GetComponent<Button>();
        }


    }
}
