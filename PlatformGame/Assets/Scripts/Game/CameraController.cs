using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Camera References")]
    public CinemachineCamera virtualCamera;
    public Transform playerSpawnPoint;
    
    [Header("Camera Settings")]
    public float setupDelay = 0.1f;
    public bool autoSetupOnStart = true;
    public bool forceCameraToPlayer = true;
    
    void Start()
    {
        if (autoSetupOnStart)
        {
            StartCoroutine(SetupCameraWithDelay());
        }
    }
    
    private System.Collections.IEnumerator SetupCameraWithDelay()
    {
        // Espera um pouco para garantir que tudo foi carregado
        yield return new WaitForSeconds(setupDelay);
        
        SetupCamera();
    }
    
    public void SetupCamera()
    {
        Debug.Log("CameraController: Configurando câmera...");
        
        // Procura pela câmera virtual se não foi configurada
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineCamera>();
            Debug.Log($"CameraController: CinemachineCamera encontrada? {virtualCamera != null}");
        }
        
        if (virtualCamera == null)
        {
            Debug.LogWarning("CameraController: CinemachineCamera não encontrada!");
            return;
        }
        
        // Procura pelo player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("CameraController: Player não encontrado!");
            return;
        }
        
        Debug.Log($"CameraController: Player encontrado: {player.name}");
        
        // Desabilita temporariamente a câmera virtual
        virtualCamera.enabled = false;
        
        // Configura a câmera para seguir o player
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
        
        // Se forçado, move a câmera para a posição do player
        if (forceCameraToPlayer)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null && player != null)
            {
                Vector3 targetPosition = player.transform.position;
                targetPosition.z = mainCamera.transform.position.z;
                mainCamera.transform.position = targetPosition;
                
                Debug.Log($"CameraController: Câmera principal movida para {targetPosition}");
            }
        }
        
        // Reabilita a câmera virtual após um frame
        StartCoroutine(ReenableCameraAfterFrame());
        
        Debug.Log("CameraController: Câmera configurada com sucesso!");
    }
    
    private System.Collections.IEnumerator ReenableCameraAfterFrame()
    {
        // Espera um frame para garantir que tudo foi configurado
        yield return null;
        
        if (virtualCamera != null)
        {
            virtualCamera.enabled = true;
            Debug.Log("CameraController: Câmera virtual reabilitada!");
        }
    }
    
    // Método público para reconectar a câmera
    public void ReconnectCamera()
    {
        Debug.Log("CameraController: Reconectando câmera...");
        SetupCamera();
    }
    
    // Método para configurar manualmente via Inspector
    [ContextMenu("Setup Camera Now")]
    public void SetupCameraNow()
    {
        SetupCamera();
    }
} 