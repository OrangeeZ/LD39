using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    public Button start;
    public Button exit;

   public void Startbutton()
    {
       
        SceneManager.LoadScene("LevelArena");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
