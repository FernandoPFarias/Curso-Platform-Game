using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterfallGroupAnimator : MonoBehaviour
{
    [System.Serializable]
    public class WaterfallPart
    {
        public Animator animator;
        [HideInInspector] public float animatorSpeed; // Será definido via randomização
    }

    [Header("Partes da Cachoeira")]
    public List<WaterfallPart> waterfallParts = new List<WaterfallPart>();

    [Header("Randomização")]
    [SerializeField] private float minSpeed = 0.8f;
    [SerializeField] private float maxSpeed = 1.2f;
    [SerializeField] private float maxStartDelay = 0.5f;

    void Start()
    {
        foreach (var part in waterfallParts)
        {
            if (part.animator != null)
            {
                part.animatorSpeed = Random.Range(minSpeed, maxSpeed);
                float delay = Random.Range(0f, maxStartDelay);
                StartCoroutine(StartWithDelay(part.animator, part.animatorSpeed, delay));
                // Desincroniza o ponto de início da animação
                part.animator.Play(0, 0, Random.Range(0f, 1f));
            }
        }
    }

    private IEnumerator StartWithDelay(Animator animator, float speed, float delay)
    {
        animator.speed = 0f;
        yield return new WaitForSeconds(delay);
        animator.speed = speed;
    }
} 