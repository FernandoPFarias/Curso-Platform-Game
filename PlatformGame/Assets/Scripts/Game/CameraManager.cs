using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera References")]
    public CinemachineCamera virtualCamera;
    public Transform playerSpawnPoint;
    
    [Header("Auto Setup")]
    public bool autoFindPlayer = true;
    public bool autoFindCamera = true;
    
    void Start()
    {
        Debug.Log("CameraManager: Iniciando configuração da câmera...");
        
        // Configura automaticamente se necessário
        if (autoFindCamera && virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineCamera>();
            Debug.Log($"CameraManager: CinemachineCamera encontrada? {virtualCamera != null}");
        }
        
        if (autoFindPlayer)
        {
            SetupCameraFollow();
        }
    }
    
    public void SetupCameraFollow()
    {
        if (virtualCamera == null)
        {
            Debug.LogWarning("CameraManager: Virtual Camera não encontrada!");
            return;
        }
        
        // Procura pelo player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log($"CameraManager: Configurando câmera para seguir {player.name}");
            virtualCamera.Follow = player.transform;
            
            // Notifica o GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetCameraFollow(player.transform);
            }
        }
        else
        {
            Debug.LogWarning("CameraManager: Player não encontrado!");
        }
    }
    
    // Método público para ser chamado por outros scripts
    public void ReconnectCamera()
    {
        Debug.Log("CameraManager: Reconectando câmera...");
        SetupCameraFollow();
    }
} 