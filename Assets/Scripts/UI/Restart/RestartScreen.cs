using System;
using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScreen : UIScreen {

    [SerializeField]
    private Button _restartButton;

    private void Start() {

        _restartButton.onClick.AddListener( OnRestartClick );
    }

    private void OnRestartClick() {

        foreach ( var each in Character.Instances ) {
            
            each.Dispose();
        }

        EventSystem.Reset();
		
        Character.Instances.Clear();
		
		SceneManager.LoadScene( "Loader" );
    }

}