using UnityEngine;
using UnityEngine.Events;

public class LeverPuzzle : MonoBehaviour
{
    public Animator animator;
    public string triggerName = "T_LeverTurn";
    public LayerMask layer;
    [SerializeField] private Vector2 boxSize = new Vector2(1, 1);
    public UnityEvent onLeverActivated;
    public UnityEvent onLeverDeactivated;

    private bool isOn = false;

    public void Interact()
    {
        Debug.Log("LeverPuzzle: Interact chamado!");
        ToggleLever();
    }

    void ToggleLever()
    {
        isOn = !isOn;
        if (animator != null)
            animator.SetTrigger(triggerName);

        if (isOn)
            onLeverActivated.Invoke();
        else
            onLeverDeactivated.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
} 