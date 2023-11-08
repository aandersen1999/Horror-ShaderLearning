using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IInteractable : MonoBehaviour
{
    [SerializeField] protected bool canInteract = true;

    [SerializeField] protected string objectName = "thing";
    [SerializeField] protected string interactText = "to use ";

    public bool CanInteract { get { return canInteract; } }
    public string ObjectName { get { return objectName; } }
    public string InteractText { get { return interactText; } }

    public abstract void Interact();
    public abstract string GetInteractText();
}
