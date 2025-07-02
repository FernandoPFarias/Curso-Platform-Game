using UnityEngine;
using System.Collections;

public class ShootingStarController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float minInterval = 5f;
    [SerializeField] private float maxInterval = 15f;
    [SerializeField] private string triggerName = "Play"; // Nome do trigger para iniciar a animação
    [SerializeField] private float animDuration = 1.5f;   // Duração da animação da estrela

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        StartCoroutine(StarRoutine());
    }

    private IEnumerator StarRoutine()
    {
        while (true)
        {
            // Espera um tempo aleatório
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Ativa a estrela (caso queira desativar entre animações)
            gameObject.SetActive(true);

            // Toca a animação
            animator.SetTrigger(triggerName);

            // Espera a animação terminar
            yield return new WaitForSeconds(animDuration);

            // Opcional: desativa a estrela até o próximo ciclo
            // gameObject.SetActive(false);
        }
    }

    // Opcional: chame este método via Animation Event no final da animação para esconder a estrela
    public void HideStar()
    {
        gameObject.SetActive(false);
    }
} 