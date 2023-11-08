using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool canInteract = true;
    public DestroyType destroyType = DestroyType.DontDestroy;

    public string interactText = "to use";

    [Tooltip("Timer to destroy object. Only applies to destroy types aftertimer and timeafterinteract")]
    [SerializeField]
    private float timer = 5.0f;

    public UnityEvent OnInteract;

    #region Monobehavior
    

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