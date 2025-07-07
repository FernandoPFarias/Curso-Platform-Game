using UnityEngine;
using UnityEditor;
using Unity.Cinemachine;

#if UNITY_EDITOR
public class CameraSetupWizard : EditorWindow
{
    private CameraSettings settings = new CameraSettings();
    private Transform playerTarget;
    private Collider2D levelBounds;
    
    [MenuItem("Tools/Camera Setup Wizard")]
    public static void ShowWindow()
    {
        GetWindow<CameraSetupWizard>("Camera Setup Wizard");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Configuração da Câmera para Jogo de Plataforma 2D", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // Referências
        GUILayout.Label("Referências", EditorStyles.boldLabel);
        playerTarget = (Transform)EditorGUILayout.ObjectField("Player Target", playerTarget, typeof(Transform), true);
        levelBounds = (Collider2D)EditorGUILayout.ObjectField("Level Bounds", levelBounds, typeof(Collider2D), true);
        
        GUILayout.Space(10);
        
        // Configurações de Follow
        GUILayout.Label("Configurações de Follow", EditorStyles.boldLabel);
        settings.deadZoneWidth = EditorGUILayout.Slider("Dead Zone Width", settings.deadZoneWidth, 0f, 1f);
        settings.deadZoneHeight = EditorGUILayout.Slider("Dead Zone Height", settings.deadZoneHeight, 0f, 1f);
        settings.softZoneWidth = EditorGUILayout.Slider("Soft Zone Width", settings.softZoneWidth, 0f, 1f);
        settings.softZoneHeight = EditorGUILayout.Slider("Soft Zone Height", settings.softZoneHeight, 0f, 1f);
        
        GUILayout.Space(10);
        
        // Configurações da Câmera
        GUILayout.Label("Configurações da Câmera", EditorStyles.boldLabel);
        settings.orthographicSize = EditorGUILayout.FloatField("Orthographic Size", settings.orthographicSize);
        settings.dampingX = EditorGUILayout.FloatField("Damping X", settings.dampingX);
        settings.dampingY = EditorGUILayout.FloatField("Damping Y", settings.dampingY);
        
        GUILayout.Space(10);
        
        // Configurações de Tela
        GUILayout.Label("Configurações de Tela", EditorStyles.boldLabel);
        settings.useScreenConfiner = EditorGUILayout.Toggle("Usar Screen Confiner", settings.useScreenConfiner);
        settings.screenEdgeBuffer = EditorGUILayout.FloatField("Screen Edge Buffer", settings.screenEdgeBuffer);
        
        GUILayout.Space(20);
        
        // Botões
        if (GUILayout.Button("Configurar Câmera"))
        {
            SetupCamera();
        }
        
        if (GUILayout.Button("Configurações Padrão para Plataforma"))
        {
            SetDefaultPlatformSettings();
        }
        
        if (GUILayout.Button("Limpar Configuração"))
        {
            ClearCameraSetup();
        }
    }
    
    private void SetupCamera()
    {
        // Criar ou encontrar CameraSetup
        CameraSetup cameraSetup = FindObjectOfType<CameraSetup>();
        if (cameraSetup == null)
        {
            GameObject setupObj = new GameObject("Camera Setup");
            cameraSetup = setupObj.AddComponent<CameraSetup>();
        }
        
        // Aplicar configurações
        cameraSetup.settings = settings;
        cameraSetup.playerTarget = playerTarget;
        cameraSetup.levelBounds = levelBounds;
        
        // Executar setup
        cameraSetup.SetupCamera();
        
        Debug.Log("Câmera configurada com sucesso!");
    }
    
    private void SetDefaultPlatformSettings()
    {
        settings.deadZoneWidth = 0.3f;
        settings.deadZoneHeight = 0.3f;
        settings.softZoneWidth = 0.7f;
        settings.softZoneHeight = 0.7f;
        settings.orthographicSize = 5f;
        settings.dampingX = 1f;
        settings.dampingY = 1f;
        settings.useScreenConfiner = true;
        settings.screenEdgeBuffer = 0.5f;
        
        // Procurar por player automaticamente
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
        
        Debug.Log("Configurações padrão aplicadas!");
    }
    
    private void ClearCameraSetup()
    {
        // Remover Virtual Cameras
        CinemachineVirtualCamera[] vcams = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var vcam in vcams)
        {
            DestroyImmediate(vcam.gameObject);
        }
        
        // Remover CameraSetup
        CameraSetup cameraSetup = FindObjectOfType<CameraSetup>();
        if (cameraSetup != null)
        {
            DestroyImmediate(cameraSetup.gameObject);
        }
        
        Debug.Log("Configuração da câmera removida!");
    }
}
#endif 