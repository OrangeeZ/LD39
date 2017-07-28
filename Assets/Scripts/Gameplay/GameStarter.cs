using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField]
    private string[] _scenesToLoad;

    void Start()
    {
        foreach (var each in _scenesToLoad)
        {
            SceneManager.LoadScene(each, LoadSceneMode.Additive);

            Debug.Log(each);
        }

        SceneManager.UnloadSceneAsync("Loader");
    }
}