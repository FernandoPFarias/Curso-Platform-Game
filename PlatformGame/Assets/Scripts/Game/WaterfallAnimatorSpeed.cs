using UnityEngine;

public class WaterfallAnimatorSpeed : MonoBehaviour
{
    [SerializeField] private float animatorSpeed = 1f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.speed = animatorSpeed;
    }
} 