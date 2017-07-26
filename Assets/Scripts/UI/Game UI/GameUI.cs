using UnityEngine;
using Assets.Scripts.UI.Game_UI;
using Assets.UniRx.Scripts.Ui;
using Packages.EventSystem;
using UniRx;
using UnityEngine.UI;

public class GameUI : UIScreen {

	[SerializeField]
	private Image _speedProgressBar;

	[SerializeField]
	private Image _distanceProgressBar;

	void Update() {

		if ( CarStateController.Instance == null ) {
			
			return;
		}

		_speedProgressBar.fillAmount = CarStateController.Instance.SpeedNormalized;
	}

}