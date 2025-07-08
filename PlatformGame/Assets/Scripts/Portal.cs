using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nomeDaProximaCena; // Defina no Inspector
    
    [Header("Portal Settings")]
    public bool useLoadingScreen = true;
    public float transitionDelay = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Portal: Player entrou no portal para {nomeDaProximaCena}");
            
            // Desabilita o input do player durante a transição
            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            
            // Usa o LoadingScreenManager se disponível e configurado
            if (useLoadingScreen && LoadingScreenManager.Instance != null)
            {
                if (LoadingScreenManager.Instance.IsProperlyConfigured())
                {
                    Debug.Log("Portal: Usando LoadingScreenManager para transição com loading screen");
                    LoadingScreenManager.Instance.LoadSceneWithLoadingScreen(nomeDaProximaCena, () => {
                        Debug.Log("Portal: Transição com loading screen concluída!");
                    });
                }
                else
                {
                    Debug.LogWarning("Portal: LoadingScreenManager não está configurado corretamente, usando fallback");
                    SceneManager.LoadScene(nomeDaProximaCena);
                }
            }
            // Fallback para SceneTransitionManager
            else if (SceneTransitionManager.Instance != null)
            {
                Debug.Log("Portal: Usando SceneTransitionManager para transição suave");
                SceneTransitionManager.Instance.TransitionToScene(nomeDaProximaCena);
            }
            // Fallback para carregamento direto
            else
            {
                Debug.LogWarning("Portal: Nenhum sistema de transição encontrado, carregando cena diretamente");
                SceneManager.LoadScene(nomeDaProximaCena);
            }
        }
    }
}
