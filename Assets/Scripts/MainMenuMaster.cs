using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMaster : MonoBehaviour
{
    private readonly string startGameScene = "Bedroom";

    [SerializeField] private GameObject BlackOverlay;

    private void Awake()
    {
        BlackOverlay.SetActive(false);
    }

    public void OnStartGame()
    {
        BlackOverlay.SetActive(true);
        SceneManager.LoadSceneAsync(startGameScene);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
