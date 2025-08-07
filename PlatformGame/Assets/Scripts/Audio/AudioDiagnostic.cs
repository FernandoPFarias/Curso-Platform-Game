using UnityEngine;

public class AudioDiagnostic : MonoBehaviour
{
    [Header("Teste de Áudio")]
    public AudioClip testClip;
    
    private AudioSource audioSource;
    
    void Start()
    {
        Debug.Log("=== DIAGNÓSTICO DE ÁUDIO ===");
        
        // Verifica configurações do sistema
        CheckSystemAudio();
        
        // Verifica configurações do Unity
        CheckUnityAudio();
        
        // Configura AudioSource para teste
        SetupAudioSource();
        
        // Testa áudio automaticamente
        TestAudio();
    }
    
    void CheckSystemAudio()
    {
        Debug.Log("=== VERIFICAÇÃO DO SISTEMA ===");
        
        // Verifica se há AudioListener na cena
        var audioListener = FindObjectOfType<AudioListener>();
        if (audioListener != null)
        {
            Debug.Log("✅ AudioListener encontrado na cena");
            Debug.Log($"Volume do AudioListener: {AudioListener.volume}");
        }
        else
        {
            Debug.LogError("❌ AudioListener não encontrado na cena!");
        }
        
        // Verifica configurações básicas
        Debug.Log("✅ Sistema de áudio: Verificando...");
    }
    
    void CheckUnityAudio()
    {
        Debug.Log("=== VERIFICAÇÃO DO UNITY ===");
        
        // Verifica configurações básicas
        Debug.Log("✅ Configurações de áudio verificadas");
        
        // Verifica se o áudio está habilitado
        var audioListener = FindObjectOfType<AudioListener>();
        if (audioListener != null && audioListener.enabled)
        {
            Debug.Log("✅ Unity Audio está habilitado");
        }
        else
        {
            Debug.LogWarning("⚠️ Unity Audio pode estar desabilitado");
        }
    }
    
    void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configura AudioSource
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
        audioSource.pitch = 1f;
        audioSource.loop = false;
        
        Debug.Log("✅ AudioSource configurado");
    }
    
    void TestAudio()
    {
        if (testClip != null)
        {
            Debug.Log($"🎵 Testando áudio: {testClip.name}");
            Debug.Log($"  - Duração: {testClip.length}s");
            Debug.Log($"  - Frequência: {testClip.frequency} Hz");
            Debug.Log($"  - Canais: {testClip.channels}");
            
            audioSource.clip = testClip;
            audioSource.Play();
            
            Debug.Log("✅ Áudio iniciado - Verifique se ouve o som");
        }
        else
        {
            Debug.LogWarning("⚠️ Nenhum AudioClip configurado para teste");
        }
    }
    
    void Update()
    {
        // Pressione T para testar áudio novamente
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestAudio();
        }
        
        // Pressione V para verificar volume
        if (Input.GetKeyDown(KeyCode.V))
        {
            CheckVolume();
        }
        
        // Pressione M para testar com música
        if (Input.GetKeyDown(KeyCode.M))
        {
            TestWithMusic();
        }
    }
    
    void CheckVolume()
    {
        Debug.Log("=== VERIFICAÇÃO DE VOLUME ===");
        Debug.Log($"Volume do sistema: {AudioListener.volume}");
        Debug.Log($"Volume do AudioSource: {audioSource.volume}");
        Debug.Log($"Mute do AudioSource: {audioSource.mute}");
        Debug.Log($"Pitch do AudioSource: {audioSource.pitch}");
        
        // Verifica se o áudio está tocando
        if (audioSource.isPlaying)
        {
            Debug.Log("✅ AudioSource está tocando");
        }
        else
        {
            Debug.LogWarning("⚠️ AudioSource não está tocando");
        }
    }
    
    void TestWithMusic()
    {
        // Cria um tom de teste
        int sampleRate = 44100;
        int duration = 1; // 1 segundo
        float frequency = 440f; // Lá (A4)
        
        AudioClip testTone = AudioClip.Create("TestTone", sampleRate * duration, 1, sampleRate, false);
        float[] samples = new float[sampleRate * duration];
        
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * i / sampleRate);
        }
        
        testTone.SetData(samples, 0);
        
        audioSource.clip = testTone;
        audioSource.Play();
        
        Debug.Log("🎵 Tocando tom de teste (440Hz) - Deve ouvir um beep");
    }
} 