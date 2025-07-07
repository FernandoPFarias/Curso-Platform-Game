using UnityEngine;
using Unity.Cinemachine;

[System.Serializable]
public class CameraSettings
{
    [Header("Follow Settings")]
    public float deadZoneWidth = 0.5f;
    public float deadZoneHeight = 0.5f;
    public float softZoneWidth = 0.8f;
    public float softZoneHeight = 0.8f;
    
    [Header("Camera Settings")]
    public float orthographicSize = 5f;
    public float dampingX = 1f;
    public float dampingY = 1f;
    
    [Header("Screen Settings")]
    public bool useScreenConfiner = true;
    public float screenEdgeBuffer = 0.1f;
}

public class CameraSetup : MonoBehaviour
{
    [Header("Camera Configuration")]
    public CameraSettings settings = new CameraSettings();
    
    [Header("References")]
    public Transform playerTarget;
    public Collider2D levelBounds;
    
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer framingTransposer;
    private CinemachineConfiner2D confiner;
    
    [ContextMenu("Setup Camera")]
    public void SetupCamera()
    {
        // Verificar se o Cinemachine está instalado
        if (!IsCinemachineAvailable())
        {
            Debug.LogError("Cinemachine não está instalado! Instale via Package Manager.");
            return;
        }
        
        // Configurar câmera principal
        SetupMainCamera();
        
        // Criar ou configurar Virtual Camera
        SetupVirtualCamera();
        
        // Configurar confiner se necessário
        if (settings.useScreenConfiner)
        {
            SetupConfiner();
        }
        
        Debug.Log("Câmera configurada com sucesso!");
    }
    
    private bool IsCinemachineAvailable()
    {
        return typeof(CinemachineVirtualCamera) != null;
    }
    
    private void SetupMainCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            mainCamera = cameraObj.AddComponent<Camera>();
        }
        
        // Configurar câmera para 2D
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = settings.orthographicSize;
        mainCamera.backgroundColor = Color.black;
        
        // Adicionar CinemachineBrain se não existir
        CinemachineBrain brain = mainCamera.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            brain = mainCamera.gameObject.AddComponent<CinemachineBrain>();
        }
    }
    
    private void SetupVirtualCamera()
    {
        // Procurar por Virtual Camera existente
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        
        if (virtualCamera == null)
        {
            // Criar nova Virtual Camera
            GameObject vcamObj = new GameObject("CM vcam1");
            virtualCamera = vcamObj.AddComponent<CinemachineVirtualCamera>();
        }
        
        // Configurar Virtual Camera
        virtualCamera.Priority = 10;
        
        // Configurar Follow
        if (playerTarget != null)
        {
            virtualCamera.Follow = playerTarget;
        }
        else
        {
            // Procurar por player automaticamente
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                virtualCamera.Follow = player.transform;
                playerTarget = player.transform;
            }
        }
        
        // Configurar Framing Transposer
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (framingTransposer == null)
        {
            framingTransposer = virtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
        }
        
        // Aplicar configurações
        framingTransposer.m_DeadZoneWidth = settings.deadZoneWidth;
        framingTransposer.m_DeadZoneHeight = settings.deadZoneHeight;
        framingTransposer.m_SoftZoneWidth = settings.softZoneWidth;
        framingTransposer.m_SoftZoneHeight = settings.softZoneHeight;
        framingTransposer.m_XDamping = settings.dampingX;
        framingTransposer.m_YDamping = settings.dampingY;
        
        // Configurar tamanho ortográfico
        virtualCamera.m_Lens.OrthographicSize = settings.orthographicSize;
    }
    
    private void SetupConfiner()
    {
        confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        if (confiner == null)
        {
            confiner = virtualCamera.gameObject.AddComponent<CinemachineConfiner2D>();
        }
        
        if (levelBounds == null)
        {
            Debug.LogWarning("CameraSetup: Nenhum Collider2D de limite foi atribuído ao Confiner. Arraste manualmente no Inspector o Collider2D de limite no campo do CinemachineConfiner2D!");
        }
    }
    
    private void CreateScreenConfiner()
    {
        // Criar objeto para o confiner
        GameObject confinerObj = new GameObject("Screen Confiner");
        confinerObj.transform.SetParent(transform);
        
        // Adicionar CompositeCollider2D
        CompositeCollider2D compositeCollider = confinerObj.AddComponent<CompositeCollider2D>();
        
        // Adicionar Rigidbody2D (necessário para CompositeCollider2D)
        Rigidbody2D rb = confinerObj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        
        // Criar polígonos para as bordas da tela
        CreateScreenEdgeColliders(confinerObj);
        
        // Configurar confiner
       //    confiner.m_BoundingShape2D = compositeCollider;
    }
    
    private void CreateScreenEdgeColliders(GameObject parent)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;
        
        float screenHeight = mainCamera.orthographicSize * 2;
        float screenWidth = screenHeight * mainCamera.aspect;
        
        // Criar 4 bordas da tela
        Vector2[] edges = new Vector2[]
        {
            new Vector2(-screenWidth/2 - settings.screenEdgeBuffer, -screenHeight/2 - settings.screenEdgeBuffer), // Esquerda
            new Vector2(screenWidth/2 + settings.screenEdgeBuffer, -screenHeight/2 - settings.screenEdgeBuffer),  // Direita
            new Vector2(-screenWidth/2 - settings.screenEdgeBuffer, screenHeight/2 + settings.screenEdgeBuffer),  // Baixo
            new Vector2(screenWidth/2 + settings.screenEdgeBuffer, screenHeight/2 + settings.screenEdgeBuffer)   // Cima
        };
        
        for (int i = 0; i < 4; i++)
        {
            GameObject edge = new GameObject($"Edge_{i}");
            edge.transform.SetParent(parent.transform);
            
            EdgeCollider2D edgeCollider = edge.AddComponent<EdgeCollider2D>();
            edgeCollider.usedByComposite = true;
            
            // Definir pontos da borda
            Vector2[] points = new Vector2[2];
            switch (i)
            {
                case 0: // Esquerda
                    points[0] = new Vector2(edges[0].x, edges[0].y);
                    points[1] = new Vector2(edges[0].x, edges[2].y);
                    break;
                case 1: // Direita
                    points[0] = new Vector2(edges[1].x, edges[1].y);
                    points[1] = new Vector2(edges[1].x, edges[3].y);
                    break;
                case 2: // Baixo
                    points[0] = new Vector2(edges[0].x, edges[0].y);
                    points[1] = new Vector2(edges[1].x, edges[1].y);
                    break;
                case 3: // Cima
                    points[0] = new Vector2(edges[2].x, edges[2].y);
                    points[1] = new Vector2(edges[3].x, edges[3].y);
                    break;
            }
            edgeCollider.points = points;
        }
    }
    
    // Método para ajustar configurações em tempo de execução
    public void UpdateCameraSettings(CameraSettings newSettings)
    {
        settings = newSettings;
        
        if (framingTransposer != null)
        {
            framingTransposer.m_DeadZoneWidth = settings.deadZoneWidth;
            framingTransposer.m_DeadZoneHeight = settings.deadZoneHeight;
            framingTransposer.m_SoftZoneWidth = settings.softZoneWidth;
            framingTransposer.m_SoftZoneHeight = settings.softZoneHeight;
            framingTransposer.m_XDamping = settings.dampingX;
            framingTransposer.m_YDamping = settings.dampingY;
        }
        
        if (virtualCamera != null)
        {
            virtualCamera.m_Lens.OrthographicSize = settings.orthographicSize;
        }
    }
    
    // Método para adicionar shake à câmera
    public void AddCameraShake(float intensity, float duration)
    {
        if (virtualCamera != null)
        {
            CinemachineImpulseSource impulseSource = virtualCamera.GetComponent<CinemachineImpulseSource>();
            if (impulseSource == null)
            {
                impulseSource = virtualCamera.gameObject.AddComponent<CinemachineImpulseSource>();
            }
            
            impulseSource.GenerateImpulse(intensity);
        }
    }
} 