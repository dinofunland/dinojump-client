using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterLobby()
    {
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    public void EnterLobby(string code)
    {
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Main");
    }
}
