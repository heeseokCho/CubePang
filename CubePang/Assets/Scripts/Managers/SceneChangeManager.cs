using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    static SceneChangeManager Instance = null;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadMainGameScene()
    {
        SceneManager.LoadSceneAsync("MainGameScene");
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadSceneAsync("LobbyScene");
    }

    public void QuitScene()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
