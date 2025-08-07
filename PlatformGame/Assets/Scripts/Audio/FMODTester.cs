using UnityEngine;
using FMODUnity;

public class FMODTester : MonoBehaviour
{
    [Header("Eventos para Testar")]
    [Tooltip("Arraste eventos do FMOD Studio aqui")]
    public EventReference musicaExploracao;
    public EventReference musicaSantuario;
    public EventReference jumpSound;
    public EventReference coinSound;
    
    [Header("Configurações")]
    public bool testOnStart = true;
    public bool showDebugInfo = true;
    
    void Start()
    {
        if (showDebugInfo)
        {
            Debug.Log("=== TESTE DE CONFIGURAÇÃO FMOD ===");
            CheckFMODSetup();
        }
        
        if (testOnStart)
        {
            TestFMODEvents();
        }
    }
    
    void CheckFMODSetup()
    {
        Debug.Log("=== VERIFICAÇÃO DE CONFIGURAÇÃO ===");
        
        // Verifica se o FMOD está funcionando
        try
        {
            var studioSystem = RuntimeManager.StudioSystem;
            Debug.Log("✅ FMOD Studio System está funcionando");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Erro no FMOD Studio System: {e.Message}");
        }
        
        // Verifica se há banks carregados
        CheckBankFiles();
        
        // Verifica eventos configurados
        CheckEventReferences();
    }
    
    void CheckBankFiles()
    {
        Debug.Log("=== VERIFICAÇÃO DE BANKS ===");
        
        string fmodPath = Application.dataPath + "/Plugins/FMOD/";
        if (System.IO.Directory.Exists(fmodPath))
        {
            string[] bankFiles = System.IO.Directory.GetFiles(fmodPath, "*.bank");
            Debug.Log($"Arquivos .bank encontrados: {bankFiles.Length}");
            
            foreach (string file in bankFiles)
            {
                Debug.Log($"  - {System.IO.Path.GetFileName(file)}");
            }
            
            if (bankFiles.Length == 0)
            {
                Debug.LogError("❌ NENHUM ARQUIVO .BANK ENCONTRADO!");
                Debug.LogError("Você precisa gerar os banks no FMOD Studio");
            }
        }
        else
        {
            Debug.LogError($"❌ Pasta FMOD não encontrada: {fmodPath}");
        }
    }
    
    void CheckEventReferences()
    {
        Debug.Log("=== VERIFICAÇÃO DE EVENTOS ===");
        
        // Verifica eventos configurados
        if (!musicaExploracao.IsNull)
        {
            Debug.Log($"✅ Música Exploração: {musicaExploracao.Path}");
        }
        else
        {
            Debug.LogWarning("⚠️ Música Exploração não configurada");
        }
        
        if (!musicaSantuario.IsNull)
        {
            Debug.Log($"✅ Música Santuário: {musicaSantuario.Path}");
        }
        else
        {
            Debug.LogWarning("⚠️ Música Santuário não configurada");
        }
        
        if (!jumpSound.IsNull)
        {
            Debug.Log($"✅ Jump Sound: {jumpSound.Path}");
        }
        else
        {
            Debug.LogWarning("⚠️ Jump Sound não configurado");
        }
        
        if (!coinSound.IsNull)
        {
            Debug.Log($"✅ Coin Sound: {coinSound.Path}");
        }
        else
        {
            Debug.LogWarning("⚠️ Coin Sound não configurado");
        }
    }
    
    void TestFMODEvents()
    {
        Debug.Log("=== TESTE DE EVENTOS FMOD ===");
        
        // Testa eventos configurados
        TestEvent("Música Exploração", musicaExploracao);
        TestEvent("Jump Sound", jumpSound);
        TestEvent("Coin Sound", coinSound);
    }
    
    void TestEvent(string eventName, EventReference eventRef)
    {
        if (!eventRef.IsNull)
        {
            try
            {
                RuntimeManager.PlayOneShot(eventRef);
                Debug.Log($"✅ {eventName} reproduzido com sucesso");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Erro ao reproduzir {eventName}: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"⚠️ {eventName} não configurado");
        }
    }
    
    void Update()
    {
        // Pressione 1 para testar música de exploração
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestEvent("Música Exploração", musicaExploracao);
        }
        
        // Pressione 2 para testar música de santuário
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestEvent("Música Santuário", musicaSantuario);
        }
        
        // Pressione 3 para testar som de pulo
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TestEvent("Jump Sound", jumpSound);
        }
        
        // Pressione 4 para testar som de moeda
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TestEvent("Coin Sound", coinSound);
        }
        
        // Pressione C para verificar configuração
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckFMODSetup();
        }
        
        // Pressione T para testar todos os eventos
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestFMODEvents();
        }
    }
} 