using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMaster : MonoBehaviour
{
    public event Action OnDisplayEventText;

    private TMP_Text displayText;
    private GameObject blackOverlay;

    private Coroutine timer;

    public void SetUIContainer(UIContainer cont)
    {
        displayText = cont.eventText;
        blackOverlay = cont.blackOverlay;
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

    private IEnumerator TextDisplayTimer()
    {
        displayText.alpha = 1.0f;
        yield return new WaitForSeconds(5.0f);
        displayText.alpha = 0.0f;
    }
}
