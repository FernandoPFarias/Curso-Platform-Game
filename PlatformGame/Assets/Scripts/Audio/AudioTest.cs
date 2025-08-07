using UnityEngine;
using FMODUnity;

public class AudioTest : MonoBehaviour
{
    [Header("Teste de Áudio")]
    [Tooltip("Arraste um evento do FMOD Studio aqui")]
    public EventReference testEvent;
    
    void Start()
    {
        Debug.Log("=== TESTE DE ÁUDIO ===");
        
        // Testa se o FMOD está funcionando
        try
        {
            Debug.Log("✅ FMOD Runtime Manager está disponível");
            Debug.Log($"FMOD Core System: {RuntimeManager.CoreSystem}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Erro ao acessar FMOD: {e.Message}");
        }
        
        // Testa se o evento está configurado
        if (!testEvent.IsNull)
        {
            Debug.Log($"✅ Evento configurado: {testEvent.Path}");
            TestAudio();
        }
        else
        {
            Debug.LogWarning("⚠️ Nenhum evento configurado para teste");
        }
    }
    
    void TestAudio()
    {
        try
        {
            RuntimeManager.PlayOneShot(testEvent);
            Debug.Log("✅ Áudio reproduzido com sucesso!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Erro ao reproduzir áudio: {e.Message}");
        }
    }
    
    void Update()
    {
        // Pressione ESPAÇO para testar o áudio
        if (Input.GetKeyDown(KeyCode.Space) && !testEvent.IsNull)
        {
            Debug.Log("Testando áudio...");
            TestAudio();
        }
    }
} 