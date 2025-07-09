using UnityEngine;
using UnityEngine.Events;

public class LeverInteractable : MonoBehaviour
{
    public Animator animator;
    public bool isOn = false;
    public string interactAnimation = "LeverTurn"; // Nome do trigger/animation

    public UnityEvent onLeverActivated;
    public UnityEvent onLeverDeactivated;

    public void Interact()
    {
        isOn = !isOn;
        if (animator != null)
            animator.SetTrigger(interactAnimation);

        if (isOn)
            onLeverActivated.Invoke();
        else
            onLeverDeactivated.Invoke();
    }
} 