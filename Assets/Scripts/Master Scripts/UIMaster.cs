using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIMaster : Singleton<UIMaster>
{
    public event Action OnDisplayEventText;

    [SerializeField] private TMP_Text displayText;
    [SerializeField] private GameObject blackOverlay;
    [SerializeField] private GameObject crosshairHand;

    private Coroutine timer;

    protected override void Awake()
    {
        base.Awake();

        crosshairHand.SetActive(false);
        BlackScreen(false);
    }

    private void OnEnable()
    {
        GameMaster.Instance.OnSetNewPlayer += OnSetNewPlayer;
        SceneManager.sceneLoaded += OnSceneChange;
    }

    private void OnDisable()
    {
        if (!GameMaster.InstanceIsNull())
        {
            GameMaster.Instance.OnSetNewPlayer -= OnSetNewPlayer;
        }
        SceneManager.sceneLoaded -= OnSceneChange;
    }

    public void DisplayEventMessage(string message)
    {
        displayText.text = message;
        if(timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
        timer = StartCoroutine(TextDisplayTimer());
    }

    public void BlackScreen(bool set)
    {
        blackOverlay.SetActive(set);
    }

    private void OnSetNewPlayer(Player player)
    {
        crosshairHand.SetActive(false);

        player.OnFoundInteractable += OnFindInteractable;
        player.OnLostInteractable += OnLoseInteractable;
    }

    private void OnFindInteractable()
    {
        crosshairHand.SetActive(true);
    }

    private void OnLoseInteractable()
    {
        crosshairHand.SetActive(false);
    }

    private void OnSceneChange(Scene s, LoadSceneMode mode)
    {
        
    }

    private IEnumerator TextDisplayTimer()
    {
        displayText.alpha = 1.0f;
        yield return new WaitForSeconds(5.0f);
        displayText.alpha = 0.0f;
    }

}
