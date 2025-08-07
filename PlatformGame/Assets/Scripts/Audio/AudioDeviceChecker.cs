using UnityEngine;
using System.Collections.Generic;

public class AudioDeviceChecker : MonoBehaviour
{
    [Header("Configurações de Áudio")]
    [Tooltip("Força o uso do dispositivo de áudio padrão")]
    public bool forceDefaultAudioDevice = true;
    
    void Start()
    {
        Debug.Log("=== VERIFICAÇÃO DE DISPOSITIVOS DE ÁUDIO ===");
        
        // Verifica dispositivos de áudio
        CheckAudioDevices();
        
        // Verifica configurações do Unity
        CheckUnityAudioSettings();
        
        // Testa áudio básico
        TestBasicAudio();
    }
    
    void CheckAudioDevices()
    {
        Debug.Log("=== DISPOSITIVOS DE ÁUDIO ===");
        
        // Verifica se o Unity está usando o dispositivo correto
        var audioListener = FindObjectOfType<AudioListener>();
        if (audioListener != null)
        {
            Debug.Log("✅ AudioListener encontrado");
            Debug.Log($"Volume: {AudioListener.volume}");
            Debug.Log($"Habilitado: {audioListener.enabled}");
        }
        else
        {
            Debug.LogError("❌ AudioListener não encontrado!");
        }
        
        // Verifica configurações do sistema
        Debug.Log("ℹ️ Verifique as configurações do Windows:");
        Debug.Log("  1. Clique com botão direito no ícone de som");
        Debug.Log("  2. Vá em 'Configurações de Som'");
        Debug.Log("  3. Verifique se o fone está selecionado como 'Dispositivo de Saída'");
        Debug.Log("  4. Teste o som no Windows primeiro");
    }
    
    void CheckUnityAudioSettings()
    {
        Debug.Log("=== CONFIGURAÇÕES DO UNITY ===");
        
        // Verifica se o áudio está habilitado
        Debug.Log("ℹ️ Verifique Project Settings → Audio:");
        Debug.Log("  - Disable Unity Audio: deve estar DESMARCADO");
        Debug.Log("  - System Sample Rate: 48000 Hz");
        Debug.Log("  - DSP Buffer Size: Best Performance");
        
        // Verifica se há problemas de configuração
        var audioListener = FindObjectOfType<AudioListener>();
        if (audioListener != null && !audioListener.enabled)
        {
            Debug.LogError("❌ AudioListener está DESABILITADO!");
            Debug.Log("Solução: Habilitar o AudioListener na Main Camera");
        }
    }
    
    void TestBasicAudio()
    {
        Debug.Log("=== TESTE DE ÁUDIO BÁSICO ===");
        
        // Cria um AudioSource para teste
        var audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configura para teste
        audioSource.volume = 1f;
        audioSource.pitch = 1f;
        audioSource.loop = false;
        
        // Cria um tom de teste
        CreateAndPlayTestTone(audioSource);
    }
    
    void CreateAndPlayTestTone(AudioSource audioSource)
    {
        // Cria um tom de teste (440Hz - Lá)
        int sampleRate = 44100;
        int duration = 2; // 2 segundos
        float frequency = 440f; // Lá (A4)
        
        AudioClip testTone = AudioClip.Create("TestTone", sampleRate * duration, 1, sampleRate, false);
        float[] samples = new float[sampleRate * duration];
        
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * i / sampleRate) * 0.5f;
        }
        
        testTone.SetData(samples, 0);
        
        audioSource.clip = testTone;
        audioSource.Play();
        
        Debug.Log("🎵 Tocando tom de teste (440Hz) por 2 segundos");
        Debug.Log("Se você não ouvir nada, o problema é:");
        Debug.Log("  1. Volume do sistema muito baixo");
        Debug.Log("  2. Fone não selecionado como dispositivo padrão");
        Debug.Log("  3. Unity Audio desabilitado");
        Debug.Log("  4. Driver de áudio com problema");
    }
    
    void Update()
    {
        // Pressione T para testar tom novamente
        if (Input.GetKeyDown(KeyCode.T))
        {
            var audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                CreateAndPlayTestTone(audioSource);
            }
        }
        
        // Pressione V para verificar volume
        if (Input.GetKeyDown(KeyCode.V))
        {
            CheckVolume();
        }
        
        // Pressione D para mostrar dispositivos
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShowDeviceInfo();
        }
    }
    
    void CheckVolume()
    {
        Debug.Log("=== VERIFICAÇÃO DE VOLUME ===");
        
        var audioListener = FindObjectOfType<AudioListener>();
        var audioSource = GetComponent<AudioSource>();
        
        if (audioListener != null)
        {
            Debug.Log($"AudioListener Volume: {AudioListener.volume}");
            Debug.Log($"AudioListener Enabled: {audioListener.enabled}");
        }
        
        if (audioSource != null)
        {
            Debug.Log($"AudioSource Volume: {audioSource.volume}");
            Debug.Log($"AudioSource Mute: {audioSource.mute}");
            Debug.Log($"AudioSource Playing: {audioSource.isPlaying}");
        }
        
        Debug.Log("ℹ️ Verifique também:");
        Debug.Log("  - Volume do Windows (ícone de alto-falante)");
        Debug.Log("  - Volume do fone de ouvido (se tiver controle)");
        Debug.Log("  - Se o fone está conectado corretamente");
    }
    
    void ShowDeviceInfo()
    {
        Debug.Log("=== INFORMAÇÕES DE DISPOSITIVO ===");
        Debug.Log("ℹ️ Para verificar dispositivos de áudio:");
        Debug.Log("  1. Clique com botão direito no ícone de som");
        Debug.Log("  2. Vá em 'Configurações de Som'");
        Debug.Log("  3. Em 'Som', clique em 'Dispositivos de saída'");
        Debug.Log("  4. Verifique se seu fone está listado e selecionado");
        Debug.Log("  5. Clique em 'Testar' para verificar se funciona");
    }
} 