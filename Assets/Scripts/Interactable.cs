using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool canInteract = true;
    public DestroyType destroyType = DestroyType.DontDestroy;
    [SerializeField] protected bool playAudioOnInteract = false;
    [SerializeField] protected AudioClip sfx;

    public string interactText = "to use";

    [Tooltip("Timer to destroy object. Only applies to destroy types aftertimer and timeafterinteract")]
    [SerializeField] private float timer = 5.0f;

    protected AudioSource sfxSource;

    public UnityEvent OnInteract;

    public const int InteractableLayer = 6;

    #region Monobehavior

    protected void Awake()
    {
        if (playAudioOnInteract)
        {
            if(sfx == null)
            {
                Debug.LogError($"No audio clip found for interactable object despite requesting to play one. Disabling audio for {gameObject}");
                playAudioOnInteract = false;
            }
            if(!TryGetComponent(out sfxSource))
            {
                Debug.LogWarning($"No Audio source component found for interactable object despite requesting to play audio." +
                    $" Creating AudioSource for object {gameObject}");
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
            
            sfxSource.clip = sfx;
        }
        if (gameObject.layer != InteractableLayer)
            Debug.LogWarning($"Interactable object {gameObject} is not part of the interactable layer and will not be able to be detected.");
    }

    protected void Start()
    {
        if (destroyType == DestroyType.AfterTimer)
        {
            StartCoroutine(Timer());
        }
    }

    
    #endregion

    /// <summary>
    /// Should be overridden with base at end of overwritten method
    /// </summary>
    public virtual void Interact()
    {
        OnInteract.Invoke();

        switch (destroyType)
        {
            case DestroyType.AfterInteract:
                Destroy(gameObject);
                break;
            case DestroyType.TimeAfterInteract:
                StartCoroutine(Timer());
                break;
            default:
                break;
        }
    }

    public virtual string GetInteractText()
    {
        return interactText;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}

public enum DestroyType : byte
{
    DontDestroy,
    AfterInteract,
    AfterTimer,
    TimeAfterInteract
}