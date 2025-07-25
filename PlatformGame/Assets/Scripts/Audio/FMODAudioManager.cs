using UnityEngine;
using FMODUnity;
using FMOD.Studio;

// AudioManager central para FMOD
// Arraste os eventos do FMOD Studio para os campos abaixo no Inspector
public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager Instance; // Singleton para acesso global

    [Header("Músicas (arraste eventos do FMOD)")]
    public EventReference mainTheme; // Música principal
    public EventReference bossTheme; // Música de chefe
    // Adicione mais músicas conforme necessário

    [Header("Efeitos Sonoros (arraste eventos do FMOD)")]
    public EventReference jumpSFX; // Som de pulo
    public EventReference coinSFX; // Som de moeda
    // Adicione mais efeitos conforme necessário

    private EventInstance musicInstance; // Instância da música atual

    private void Awake()
    {
        // Garante singleton
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    // Toca uma música de fundo
    public void PlayMusic(EventReference musicEvent)
    {
        if (musicInstance.isValid())
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();
    }

    // Para a música de fundo
    public void StopMusic()
    {
        if (musicInstance.isValid())
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // Toca um efeito sonoro
    public void PlaySFX(EventReference sfxEvent)
    {
        RuntimeManager.PlayOneShot(sfxEvent);
    }
} 