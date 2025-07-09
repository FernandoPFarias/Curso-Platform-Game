using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nomeDaProximaCena; // Defina no Inspector
    [Header("Portal Settings")]
    public bool useLoadingScreen = true;
    public float transitionDelay = 0.5f;
    public float requiredCenterDistance = 0.2f; // Distância máxima para considerar o player "centralizado" na porta

    private bool playerInPortal = false;
    private Transform playerTransform;
    private PlayerController playerController;

    void Update()
    {
        if (playerInPortal && playerTransform != null)
        {
            // Checa se o player está centralizado na porta
            if (Mathf.Abs(playerTransform.position.x - transform.position.x) < requiredCenterDistance)
            {
                // Checa se o botão de interação foi pressionado
                if (Input.GetButtonDown("Interact")) // Certifique-se que o botão está mapeado no Input System
                {
                    TrocarDeFase();
                }
                // (Opcional) Mostrar dica visual: "Pressione Interagir para entrar"
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInPortal = true;
            playerTransform = other.transform;
            playerController = other.GetComponent<PlayerController>();
            // (Opcional) Mostrar dica visual: "Pressione Interagir para entrar"
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInPortal = false;
            playerTransform = null;
            playerController = null;
            // (Opcional) Esconder dica visual
        }
    }

    private void TrocarDeFase()
    {
        Debug.Log($"Portal: Player usou o portal para {nomeDaProximaCena}");
        if (playerController != null)
            playerController.enabled = false;

        if (useLoadingScreen && LoadingScreenManager.Instance != null)
        {
            if (LoadingScreenManager.Instance.IsProperlyConfigured())
            {
                LoadingScreenManager.Instance.LoadSceneWithLoadingScreen(nomeDaProximaCena, () => {
                    Debug.Log("Portal: Transição com loading screen concluída!");
                });
                return;
            }
        }
        else if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.TransitionToScene(nomeDaProximaCena);
            return;
        }
        SceneManager.LoadScene(nomeDaProximaCena);
    }

    public void TryInteract()
    {
        if (playerInPortal && playerTransform != null)
        {
            if (Mathf.Abs(playerTransform.position.x - transform.position.x) < requiredCenterDistance)
            {
                TrocarDeFase();
            }
        }
    }
}
