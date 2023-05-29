using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "TestScene";
    private void Awake() {
        if (NetworkManager.Singleton != null) {
            Destroy(NetworkManager.Singleton.gameObject);
        }
        if (GameMultiplayer.Instance != null) {
            Destroy(GameMultiplayer.Instance.gameObject);
        }
    }
    public void StartHost() {
        Loader.Load(Loader.Scene.LobbyScene);
        //NetworkManager.Singleton.StartHost();
        //NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }
    public void StartServer() {
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }
    public void StartClient() {
        NetworkManager.Singleton.StartClient();
    }

    public void ShowSettings() {
        SettingsUI.Instance.Show();
    }

    public void ExitGame() {
        Application.Quit();
    }
}
